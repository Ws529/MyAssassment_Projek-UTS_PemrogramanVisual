// Lokasi: WEBSAMPLE/Global.asax.cs

using System;
using System.Web.Http; // <-- BARIS PENTING YANG HARUS DITAMBAH
using System.Web.Mvc;
using System.Web.Routing;

// Ganti namespace menjadi WEBSAMPLE
namespace WEBSAMPLE 
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Baris ini adalah yang Anda tambahkan dari kode Anda.
            // Ini memanggil file 'WebApiConfig.cs' Anda.
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // (Baris-baris di bawah ini mungkin sudah ada di file Anda, biarkan saja)
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes); 
        }
    }
}