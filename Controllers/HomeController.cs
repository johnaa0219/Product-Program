using Microsoft.AspNetCore.Mvc;
//using SQLPRODUCTCONN.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using POC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExtensionMethods;
using System.Reflection;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace POC.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestContext _context;
       
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, TestContext context)
        {
            _context = context;
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "首頁";
            return View();
        }

        public IActionResult Product()
        {
            //SqlProductConn productconn = new SqlProductConn(_configuration);
            //List<Product> products = productconn.GetProducts();
            //ViewData["Products"] = products;

            /*var noa = resource.OrderByDescending(x => x.Noa).Take(1).ToList()[0].Noa;
            
            var tt = (from s1 in _context.Product       //where ... in ...
                      where (new string[] { "C" }).Contains(s1.Typea)
                      select s1).ToList();*/
            //Dictionary類似object功能
            /*Dictionary<string, List<string>> FlavorDictionary = new Dictionary<string, List<string>>();   
            for (int i = 0; i < DistinctProduct.Count; i++)
            {
                var FilterFlavor = (from a in _context.Product
                                    select a)
                             .Where(x => x.Product1 == DistinctProduct[i])
                             .Distinct();
                List<String> ListFlavor = FilterFlavor.Select(a => a.Noa)
                                 .ToList();
                FlavorDictionary.Add(DistinctProduct[i], ListFlavor);
                ViewData["Products"] = FlavorDictionary;    //View呼叫時 用ViewBag.Products
            }*/
            double count = (from a in _context.Product     //全部資料筆數
                            select a).Count();
            var productResource = (from a in _context.Product
                            select a).Take(5).OrderBy(x => x.Noa);
            var productResult = productResource.ToList();
            var productsResource = (from a in _context.Products
                            select a).Where(x => x.Noa== productResult[0].Noa).OrderBy(x => x.Noq);
            List<Products> productsResult = null;
            if (productResult.Count > 0)
            {
                productsResult = productsResource.ToList();
            }
            ViewTModel<Product, Products> result = new ViewTModel<Product,Products>();
            result.Head = productResult;
            result.Body = productsResult;

            ViewData["Message"] = "這是產品頁";
            ViewData["Title"] = "產品";
            ViewData["nowPage"] = 1;
            ViewData["totPage"] = Math.Ceiling(count / 5);
            return View(result);    //View呼叫時 用@model POC.Models.ViewTModel 
        }
        public IActionResult Orde()
        {
            double count = (from a in _context.Orde     //全部資料筆數
                            select a).Count();
            var OrdeResource = (from a in _context.Orde
                                   select a).Take(5).OrderBy(x => x.Noa);
            var OrdeResult = OrdeResource.ToList();
            var OrdesResource = (from a in _context.Ordes
                                 select a).Where(x => x.Noa == OrdeResult[0].Noa).OrderBy(x => x.Noq);
            List<Ordes> OrdesResult = null;
            if (OrdeResult.Count>0)
            {
                OrdesResult = OrdesResource.ToList();
            }
            ViewTModel<Orde, Ordes> result = new ViewTModel<Orde,Ordes>();
            result.Head = OrdeResult;
            result.Body = OrdesResult;

            ViewData["Message"] = "這是訂單頁";
            ViewData["Title"] = "訂單";
            ViewData["nowPage"] = 1;
            ViewData["totPage"] = Math.Ceiling(count / 5);
            return View(result);    //View呼叫時 用@model POC.Models.ViewTModel 
        }
        public IActionResult Revenue()
        {
            ViewData["Message"] = "這是營收頁";
            ViewData["Title"] = "營收";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public Object GetData(string DB, string Noa, int page, int count)  //q_gtNoa
        {
            DB = DB.Left(1).ToUpper() + DB.Right(DB.Length - 1);
            Object result = new Object { };
            string genericTypeName = "POC.Models." + DB;
            Type genericType = Type.GetType(genericTypeName);
            Type hometype = typeof(HomeController);
            MethodInfo gtInfo = hometype.GetMethod("GetDataGeneric");
            MethodInfo methodInfo = gtInfo.MakeGenericMethod(genericType);

            var homeclass = new HomeController(this._configuration, this._context);
            result = methodInfo.Invoke(homeclass, new object[] { DB,Noa,page,count });   //methodInfo.Invoke( instance(實例) ,Object)
            return result;
        }
        public Array GetDataGeneric<T>(string DB, string Noa, int page, int count)  //q_gtNoa
            where T : class
        {
            Array result = null;
            var source = (from a in _context.Set<T>()
                          select a).WhereContains((Noa == null ? null : "Noa"), Noa).OrderByCol("Noa");
            if (page < 0)
            {
                result = source.ToArray();
            }
            else
            {
                result = source.Take(page * count).ToArray();
            }
            return result;
        }
        public Object GetDataWhere(string DB, string swhere)
        {
            Object result = new Object { };
            string genericTypeName = "POC.Models."+DB;      //例:DB="Products" 
                                                            //1.會找不到 要加上namespace 
                                                            //2.要注意大小寫
            Type genericType = Type.GetType(genericTypeName);   //GetType(String) 取得POC.Models.Products這個類別
            Type hometype = typeof(HomeController);             //typeof(Type) 取得HomeController這個類別
            MethodInfo gtInfo = hometype.GetMethod("GetDataWhereGeneric");   //Type.GetMethod(String) 取得HomeController底下"GetDataGeneric"這個Method
            MethodInfo methodInfo = gtInfo.MakeGenericMethod(genericType);  //將"POC.Models.Products"這個類別(genericType)帶入Method中的泛型

            var homeclass = new HomeController(this._configuration, this._context);
            result=methodInfo.Invoke(homeclass, new object[] { swhere });   //methodInfo.Invoke( instance(實例) ,Object)
            //homeclass : 你真的要呼叫的function的class建立的instance(實例)
            //new object[] { swhere } 
            //要傳入function的值 要放在一個object裡
            return result;
        }
        public Array GetDataWhereGeneric<T>(string swhere)  //q_gt
            where T : class
        {
            string[] conditions = swhere.Split(";");
            Array result = null;
            IQueryable<T> tempResult = _context.Set<T>().WhereContains(conditions[0].Split(':')[0], conditions[0].Split(':')[1]);
            for (int i = 1; i < conditions.Length; i++)
            {
                tempResult = tempResult.WhereContains(conditions[i].Split(':')[0], conditions[i].Split(':')[1]);
            }
            result = tempResult.ToArray();
            return result;
        }
        [HttpGet]
        public IActionResult GetPartial(String DB,String Noa)
        {
            DB = DB.Left(1).ToUpper() + DB.Right(DB.Length - 1);
            Object result = new object { };
            string genericTypeName = "POC.Models." + DB;
            Type genericType = Type.GetType(genericTypeName);
            Type hometype = typeof(HomeController);
            MethodInfo gtInfo = hometype.GetMethod("GetPartialGeneric");
            MethodInfo methodInfo = gtInfo.MakeGenericMethod(genericType);

            var homeclass = new HomeController(this._configuration, this._context);
            result = methodInfo.Invoke(homeclass, new object[] { DB,Noa });
            return PartialView("_" + DB, result);
            //PartialView("View檔名", 要傳至viewmodel的參數);
        }
        public List<T> GetPartialGeneric<T>(String DB, String Noa)
            where T : class
        {
            List<T> result = new List<T>();
            if (Noa != null)
            {
                result = (from a in _context.Set<T>()
                                  select a).WhereContains("Noa", Noa).OrderByCol("Noq").ToList();
            }
            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public ActionResult sqlSave(string DB,string status, string tHead, string tBody)
        {
            DB = DB.Left(1).ToUpper() + DB.Right(DB.Length - 1);
            string HeadTypeName = "POC.Models." + DB;
            string BodyTypeName = "POC.Models." + DB + "s";
            Type HeadType = Type.GetType(HeadTypeName);
            Type BodyType = Type.GetType(BodyTypeName);
            Type hometype = typeof(HomeController);
            MethodInfo gtInfo = hometype.GetMethod("sqlSaveGeneric");
            MethodInfo methodInfo = gtInfo.MakeGenericMethod(HeadType, BodyType); 
            var homeclass = new HomeController(this._configuration, this._context);
            var result = methodInfo.Invoke(homeclass, new object[] { DB, status, tHead, tBody });
            return (ActionResult)result;
        }
        public ActionResult sqlSaveGeneric<T, Ts>(string DB,string status, string tHead, string tBody)
            where T : class where Ts : class
        {
            string HeadTypeName = "POC.Models." + DB;
            string BodyTypeName = "POC.Models." + DB + "s";
            string ViewTypeName = "View" + DB + "Model";
            T Head = JsonConvert.DeserializeObject<T>(tHead);
            List<Ts> Body = JsonConvert.DeserializeObject<List<Ts>>(tBody);
            Type HeadType = Type.GetType(HeadTypeName);     //取得Type類型
            PropertyInfo HeadProperty = HeadType.GetProperty("Noa");    //取得Noa成員
            var Noa = HeadProperty.GetValue(Head);    //從Head中取得Noa成員的值
            Type BodyType = Type.GetType(BodyTypeName);
            PropertyInfo BodyNoaProperty = BodyType.GetProperty("Noa");
            PropertyInfo BodyNoqProperty = BodyType.GetProperty("Noq");
            switch (status)
            {
                case "1":
                    _context.Set<T>().Add(Head);
                    for (var i = 0; i < Body.Count; i++)
                    {
                        BodyNoaProperty.SetValue(Body[i], Noa);
                        BodyNoqProperty.SetValue(Body[i], ("000" + Convert.ToString(i + 1)).Right(3));
                        _context.Set<Ts>().Add(Body[i]);
                    }
                    _context.SaveChanges();
                    break;
                case "2":
                    _context.Set<T>().Remove(Head);
                    List<Ts> forRemoveBody = (from a in _context.Set<Ts>()
                                                  select a).WhereContains("Noa", (string)Noa).ToList();
                    if (_context.Set<Ts>().WhereContains("Noa", (string)Noa).WhereContains("Noq", "001").AsNoTracking().ToArray().Length > 0)
                    {
                        foreach (Ts body in forRemoveBody)
                        {
                            _context.Set<Ts>().Remove(body);
                        }
                    }
                    _context.SaveChanges();
                    _context.Set<T>().Add(Head);
                    for (var i = 0; i < Body.Count; i++)
                    {
                        BodyNoaProperty.SetValue(Body[i], Noa);
                        BodyNoqProperty.SetValue(Body[i], ("000" + Convert.ToString(i + 1)).Right(3));
                        _context.Set<Ts>().Add(Body[i]);
                    }
                    _context.SaveChanges();
                    break;
                case "3":
                    _context.Set<T>().Remove(Head);
                    if (_context.Set<Ts>().WhereContains("Noa", (string)Noa).WhereContains("Noq", "001").AsNoTracking().ToArray().Length > 0)
                    {
                        foreach (Ts body in Body)
                        {
                            BodyNoaProperty.SetValue(body, Noa);
                            _context.Set<Ts>().Remove(body);
                        }
                    }
                    _context.SaveChanges();
                    break;

            }
            double count = (from a in _context.Set<T>()     //全部資料筆數
                            select a).Count();
            var HeadResource = (from a in _context.Set<T>()
                            select a).Take(5).OrderByCol("Noa");
            var HeadResult = HeadResource.ToList();
            var BodyResource = (from a in _context.Set<Ts>()
                                 select a).WhereContains("Noa",(string)Noa).OrderByCol("Noq");
            List<Ts> BodyResult = null;
            if (BodyResource.ToArray().Length > 0)
            {
                BodyResult = BodyResource.ToList();
            }

            var result = new ViewTModel<T,Ts>();
            result.Head = HeadResult;
            result.Body = BodyResult;
            switch (DB)
            {
                case "Product":
                    ViewData["Message"] = "這是產品頁";
                    ViewData["Title"] = "產品";
                    break;
                case "Orde":
                    ViewData["Message"] = "這是訂單頁";
                    ViewData["Title"] = "訂單";
                    break;
            }
            ViewData[DB] = result;
            return View(DB, result);
        }
    }
}
