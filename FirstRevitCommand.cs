//1
#region
//using Autodesk.Revit.DB;
//using Autodesk.Revit.UI;
//using Autodesk.Revit.UI.Selection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FirstRevitPlugin
//{
//    // 24-12-2025
//    //https://dzen.ru/a/ZM9VpCtKUm7-AUbv
//    //Чтобы добавить атрибут транзакции, добавим строку
//    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
//    //перед классом.
//    public class FirstRevitCommand : IExternalCommand
//    {
//        /// Но это ещё не всё.Все надстройки для Revit должны иметь уникальный идентификатор(Guid),
//        // совпадающий в коде и в addin-файле, а также атрибут настроек транзакции.
//        //Чтобы добавить Guid, добавим следующую строку в начало класса:

//        static AddInId addinId = new AddInId(new Guid("2C89D91B-051D-45B0-977B-EE0DCD637435"));

//        //Guid {2C89D91B-051D-45B0-977B-EE0DCD637435} — это случайный Guid моего плагина.
//        //Вам нужно сделать свой, таким образом: Средства — Создать Guid:
//        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
//            UIDocument uidoc = commandData.Application.ActiveUIDocument;
//            Document doc = uidoc.Document;
//            Reference myRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент для вывода его Id");
//            Element element = doc.GetElement(myRef);

//            ElementId id = element.Id;
//            TaskDialog.Show("Hello world!", id.ToString());
//            //throw new NotImplementedException();
//            return Result.Succeeded;
//        }
//    }
//}
#endregion
//2
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Text;

namespace FirstRevitPlugin
{
    [Autodesk.Revit.Attributes.Transaction(
        Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FirstRevitCommand : IExternalCommand
    {
        static AddInId addinId = new AddInId(
            new Guid("2C89D91B-051D-45B0-977B-EE0DCD637435"));

        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference myRef = uidoc.Selection.PickObject(
                ObjectType.Element,
                "Выберите элемент электросети");

            Element element = doc.GetElement(myRef);
            ElementId id = element.Id;

            // TODO: здесь укажи точные имена параметров
            // как они отображаются в свойствах Revit
            string cableNameParam = "Имя кабеля";   // пример
            string markParam = "Марка";        // пример

            string cableName = GetStringParam(element, cableNameParam);
            string markName = GetStringParam(element, markParam);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Id элемента: {id.IntegerValue}");
            sb.AppendLine($"Имя кабеля: {cableName}");
            sb.AppendLine($"Марка элемента: {markName}");

            TaskDialog.Show("Электросети", sb.ToString());

            return Result.Succeeded;
        }

        private string GetStringParam(Element el, string paramName)
        {
            Parameter p = el.LookupParameter(paramName);
            if (p == null)
                return $"[параметр \"{paramName}\" не найден]";

            if (p.StorageType == StorageType.String)
                return p.AsString() ?? "[пусто]";

            return p.AsValueString() ?? "[пусто]";
        }
    }
}
