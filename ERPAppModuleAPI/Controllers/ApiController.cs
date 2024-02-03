using Microsoft.AspNetCore.Mvc;

namespace ERPAppModule.Controllers;

[Controller]
public abstract class ApiController
{
    [ActionContext]
    public ActionContext ActionContext { get; set; }
}