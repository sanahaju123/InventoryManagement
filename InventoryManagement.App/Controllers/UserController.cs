using ClosedXML.Excel;
using InventoryManagement.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> DeleteCategoryById([FromRoute] int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "Category/DeleteCategory";
                using (var Response = await client.GetAsync(endpoint))
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
            #endregion

            #region Product
            [HttpGet]
            public ActionResult AddProduct()
            {
                Product product = new Product();
                return View(product);
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
                    using (var Response = await client.GetAsync(endpoint))
                    {
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
            public async Task<ActionResult> UpdateProductDetails([FromRoute] int id)
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
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = apiBaseUrl + "Product/DeleteProduct";
                    using (var Response = await client.GetAsync(endpoint))
                    {
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
        }
    } 
