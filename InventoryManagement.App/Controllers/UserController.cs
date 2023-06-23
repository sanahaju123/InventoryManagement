using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using InventoryManagement.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InventoryManagement.App.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public const string SessionKeyName = "_Name";
        public UserController(IConfiguration configuration)
        {
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public IActionResult Index()
        {
            return View();
        }
        #region User

        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            //HttpContext.Session.Remove("Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Signup");
        }

        [HttpPost]
        public async Task<ActionResult> Login(User user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    string endpoint = apiBaseUrl + "User/LoginUser";
                    using (var Response = await client.PostAsync(endpoint, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            HttpContext.Session.SetString("Username", user.UserName);
                            return RedirectToAction("GetAllProducts");
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("GetAllProducts");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(User user)

        {
            List<User> list = new List<User>();
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "User/CreateUser";
                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContext.Session.SetString(SessionKeyName, "True");
                        TempData["CreateUer"] = JsonConvert.SerializeObject(user);
                        return RedirectToAction("GetAllCategories");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "User/GetAllUsers";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<IEnumerable<User>>(jsonString);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }
        #endregion

        #region Category
        private string CategoryToCsv(IEnumerable<Category> data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id,Name,Description,IsDeleted");


            foreach (var item in data)
            {
                sb.AppendLine($"{item.Id},{item.Name},{item.Description},{item.IsDeleted}");
            }

            return sb.ToString();
        }


        public async Task<FileContentResult> ExportCSV_CategoryData()
        {
            var jsonString = "";
            List<Category> res = new List<Category>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/GetAllCategorys";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        jsonString = Response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                        res = result.ToList();
                    }
                }
            }

            var data = res;
            var csvContent = CategoryToCsv(data);
            var fileName = "CategoryData.csv";
            var contentType = "text/csv";
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return File(Encoding.UTF8.GetBytes(csvContent), contentType);
        }




        [HttpGet]
        public ActionResult AddCategory()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)

        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "Category/CreateCategory";
                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["CreateUer"] = JsonConvert.SerializeObject(category);
                        return RedirectToAction("GetAllCategories");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/GetAllCategorys";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateCategoryDetails([FromRoute] int id)
        {
            using (HttpClient client = new HttpClient())
            {                
                string endpoint = apiBaseUrl + "Category/GetCategoryById/" + id;
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Category>(jsonString);
                        
                        return View("GetAllCategories");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCategoryDetails([FromRoute] int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/GetCategoryById/" + id;
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Category>(jsonString);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCategoryDetails(Category Model)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/UpdateCategory";
                StringContent content = new StringContent(JsonConvert.SerializeObject(Model), Encoding.UTF8, "application/json");
                using (var Response = await client.PutAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Category>(jsonString);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Username", "Username Already Exist");
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/DeleteCategory/" + id;
                using (var Response = await client.DeleteAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);

                        return RedirectToAction("GetAllCategories");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<FileContentResult> ExportExcel_CategoryData()
        {
            List<Category> res = new List<Category>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/GetAllCategorys";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                        res = result.ToList();
                    }
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Category");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Id";
                    worksheet.Cell(currentRow, 2).Value = "Name";
                    worksheet.Cell(currentRow, 3).Value = "Description";
                    worksheet.Cell(currentRow, 4).Value = "IsDeleted";
                    foreach (var product in res)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = product.Id;
                        worksheet.Cell(currentRow, 2).Value = product.Name;
                        worksheet.Cell(currentRow, 3).Value = product.Description;
                        worksheet.Cell(currentRow, 4).Value = product.IsDeleted;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Category.xlsx");
                    }
                }
            }
        }

        public async Task<FileContentResult> ExportExcel_ProductData()
        {
            List<Product> res = new List<Product>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Product/GetAllProducts";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);
                        res = result.ToList();
                    }
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Products");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Id";
                    worksheet.Cell(currentRow, 2).Value = "Name";
                    worksheet.Cell(currentRow, 3).Value = "CategoryId";
                    worksheet.Cell(currentRow, 4).Value = "IsDeleted";
                    foreach (var product in res)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = product.Id;
                        worksheet.Cell(currentRow, 2).Value = product.Name;
                        worksheet.Cell(currentRow, 3).Value = product.CategoryId;
                        worksheet.Cell(currentRow, 4).Value = product.IsDeleted;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "products.xlsx");
                    }
                }
            }

        }
        #endregion

        #region Product

        private string ProductToCsv(IEnumerable<Product> data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id,Name,Price,CategoryId,IsDeleted");


            foreach (var item in data)
            {
                sb.AppendLine($"{item.Id},{item.Name},{item.CategoryId},{item.price},{item.IsDeleted}");
            }

            return sb.ToString();
        }


        public async Task<FileContentResult> ExportCSV_ProductData()
        {
            var jsonString = "";
            List<Product> res = new List<Product>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Product/GetAllProducts";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        jsonString = Response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);
                        res = result.ToList();
                    }
                }
            }

            var data = res;
            var csvContent = ProductToCsv(data);
            var fileName = "ProductData.csv";
            var contentType = "text/csv";
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return File(Encoding.UTF8.GetBytes(csvContent), contentType);
        }


        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            using (HttpClient client = new HttpClient())
            {
                string category = apiBaseUrl + "Category/GetAllCategorys";
                using (var result = await client.GetAsync(category))
                {
                    var jsonString = result.Content.ReadAsStringAsync().Result;
                    var res = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                    ViewBag.Categories = res;
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)

        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "Product/CreateProduct";
                using (var Response = await client.PostAsync(endpoint, content))
                {

                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("GetAllProducts");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Product/GetAllProducts";
                string category = apiBaseUrl + "Category/GetAllCategorys";

                using (var Response = await client.GetAsync(endpoint))
                {
                    using (var result = await client.GetAsync(category))
                    {
                        var jsonString = result.Content.ReadAsStringAsync().Result;
                        var res = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                        TempData["categoryData"] = res;
                    }
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductDetails([FromRoute] int id)
        {
            using (HttpClient client = new HttpClient())
            {
                List<Category> myList = new List<Category>();

                string category = apiBaseUrl + "Category/GetAllCategorys";
                using (var Response = await client.GetAsync(category))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<IEnumerable<Category>>(jsonString);
                        myList = response.ToList();
                    }
                }
                string json = JsonConvert.SerializeObject(myList);
                string endpoint = apiBaseUrl + "Product/GetProductById/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Product>(jsonString);
                        //string json = JsonConvert.SerializeObject(res);
                        HttpContext.Session.SetString("ListData", json);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetProductDetails([FromRoute] int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Product/GetProductById/" + id;
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Product>(jsonString);

                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateProductDetails(Product Model)
        {
            using (HttpClient client = new HttpClient())
            {
               
                string endpoint = apiBaseUrl + "Product/UpdateProduct";
                StringContent content = new StringContent(JsonConvert.SerializeObject(Model), Encoding.UTF8, "application/json");
                using (var Response = await client.PutAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var jsonString = Response.Content.ReadAsStringAsync().Result;

                        var response = JsonConvert.DeserializeObject<Product>(jsonString);
                        return View(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = apiBaseUrl + "Product/DeleteProduct/" + id;
                    using (var Response = await client.DeleteAsync(endpoint))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = Response.Content.ReadAsStringAsync().Result;

                            var response = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonString);

                            return RedirectToAction("GetAllProducts");
                        }
                        else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            return View();
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
