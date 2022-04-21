using Structurizr;
using Structurizr.Api;

namespace c4_model_design{

    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 69599;
            const string apiKey = "b6264901-97fc-4394-ac35-9c7b028e896c";
            const string apiSecret = "addaf4d4-4884-4958-8327-4b8473a5b2de";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            //nombre
            Workspace workspace = new Workspace("Ksero App", "Sistema intermediario de bodegueros-mayoristas");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto

            //sistemas
            SoftwareSystem payU = model.AddSoftwareSystem("PayU", "Plataforma que ofrece una REST API de pago.");
            SoftwareSystem gmail = model.AddSoftwareSystem("GoogleEmail", "Plataforma que ofrece una REST API de correo.");
            SoftwareSystem googleCalendar = model.AddSoftwareSystem("GoogleCalendar", "Plataforma que ofrece una REST API de calendario.");
            
            SoftwareSystem kseroApp = model.AddSoftwareSystem("Ksero App", "Aplicacion de abastecimiento a bodegas");

            
            Person adminUser = model.AddPerson("Administator", "Usuario que administra la app");
            Person wholesaleUser = model.AddPerson("Wholesaler", "Usuario mayorista que ofrece los productos");
            Person retailUser = model.AddPerson("Retailer", "Usuario minorista que realiza la orden de pedido ");
           

            
            adminUser.Uses(kseroApp, "Administra la aplicacion");
            wholesaleUser.Uses(kseroApp, "Modifica las caracteristicas de sus productos");
            retailUser.Uses(kseroApp, "Realiza los pedidos ");
           
    
            kseroApp.Uses(payU, "Usa la API de PayU ");
            kseroApp.Uses(gmail, "Usa la API de Gmail ");
            kseroApp.Uses(googleCalendar, "Usa la API de GoogleCalendar");


            SystemContextView contextView = viewSet.CreateSystemContextView(kseroApp, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags

            adminUser.AddTags("Usuario que administra");
            wholesaleUser.AddTags("Usurio que modifica  sus productos");
            retailUser.AddTags("Usuario que realiza ordenes");
            kseroApp.AddTags("Ksero App");
            payU.AddTags("PayU");
            gmail.AddTags("GoogleEmail");
            googleCalendar.AddTags("GoogleCalendar");
          

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Usuario que administra")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Usurio que modifica  sus productos")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Usuario que realiza ordenes")
                {Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person});
            styles.Add(new ElementStyle("Ksero App")
                {Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("PayU")
                {Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("GoogleEmail")
                {Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("GoogleCalendar")
                {Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox});

            
            // 2. Diagrama de Contenedores

            Container mobileApplication = kseroApp.AddContainer("Mobile App", "", "Angular");
            Container webApplication = kseroApp.AddContainer("Web App", "", "Angular");
            Container landingPage = kseroApp.AddContainer("Landing Page", "", "Angular");
            Container apiRest = kseroApp.AddContainer("API Rest", "API Rest", "NodeJS (NestJS) port 8080");
            Container dataBase = kseroApp.AddContainer("DataBase", "", "SQL Server");
            
             
            
            //bounded context
            
            Container retailerContext = kseroApp.AddContainer("RetailerContext", "Contexto de los bodegueros");
            Container paymentProcessContext = kseroApp.AddContainer("PaymentProcessContext", "Contexto de pago");
            Container wholeSalerContext = kseroApp.AddContainer("WholeSalerContext", "Contexto de los mayoristas");
            Container notifyContext = kseroApp.AddContainer("NotifyContext", "Contexto de notificacion a correos");
            Container saleRecordContext = kseroApp.AddContainer("SaleRecordContext", "Contexto del registro de pedidos ");
            
            //others
            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            landingPage.Uses(apiRest, "API Request", "JSON/HTTPS");
            
            //usuarios uses
            //mobile app
            adminUser.Uses(mobileApplication, "", "");
            wholesaleUser.Uses(mobileApplication, "", "");
            retailUser.Uses(mobileApplication, "", "");
            
            //web app
            adminUser.Uses(webApplication, "", "");
            wholesaleUser.Uses(webApplication, "", "");
            retailUser.Uses(webApplication, "", "");
            
            //landing page
            adminUser.Uses(landingPage, "", "");
            wholesaleUser.Uses(landingPage, "", "");
            retailUser.Uses(landingPage, "", "");
            
            
            //Api uses BC
            apiRest.Uses(retailerContext, "", "");
            apiRest.Uses(paymentProcessContext, "", "");
            apiRest.Uses(wholeSalerContext, "", "");
            apiRest.Uses(notifyContext, "", "");
            apiRest.Uses(saleRecordContext, "", "");
            //database
            wholeSalerContext.Uses(dataBase, "", "JDBC");
            retailerContext.Uses(dataBase, "", "JDBC");
            notifyContext.Uses(dataBase, "", "JDBC");
            paymentProcessContext.Uses(dataBase, "", "JDBC");
            saleRecordContext.Uses(dataBase, "", "JDBC");
            
            //app use aditionals
            paymentProcessContext.Uses(payU, "API Request", "JSON/HTTPS");
            notifyContext.Uses(gmail, "API Request", "JSON/HTTPS");
            wholeSalerContext.Uses(googleCalendar, "API Request", "JSON/HTTPS");
            

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            dataBase.AddTags("Database");
            
            wholeSalerContext.AddTags("WholeSaleContext");
            retailerContext.AddTags("RetailerContext");
            notifyContext.AddTags("NotifyContext");
            paymentProcessContext.AddTags("PaymentProcessContext");
            saleRecordContext.AddTags("SaleRecordContext");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            
            styles.Add(new ElementStyle("WholeSaleContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RetailerContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("NotifyContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentProcessContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SaleRecordContext") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(kseroApp, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();
            
             
            //Components
            //notify context
            Component emailComponent = apiRest.AddComponent("EmailComponent", "Envia mensaje a los usuarios");
            //RetailerContext 
            Component orderSummaryComponent = apiRest.AddComponent("OrderSummaryComponent", "Provee el registro de pedidos ");
            Component accountRetailerController = apiRest.AddComponent("AccountRetailerController", "Modifica informacion de la propia cuenta(boguero)");
           
            //WholeSalerContext
            Component modifyComponent = apiRest.AddComponent("ModifyComponent", "Modifica informacion de sus productos");
            Component aproveComponent = apiRest.AddComponent("AproveComponent", "Registra la venta validada");
            Component trackingComponent = apiRest.AddComponent("TrackingComponent", "Especifica el estado de tracking");
            Component accountWholeSaleController = apiRest.AddComponent("AccountWholeSaleController", "Modifica informacion de la propia cuenta(vendedor)");
            //payment context
            Component securityComponent = apiRest.AddComponent("SecurityComponent", "Provee funcionalidad relacionada a la verificacion de pagos y usuarios");
            Component payController = apiRest.AddComponent("PayController", "Realiza y verifica el pago del pedido");
           //SaleRecordContext 
           Component saleRecordSummaryController = apiRest.AddComponent("SaleRecordSummaryController", "Provee el registro de pedidos realizados");
           Component orderDetailComponent = apiRest.AddComponent("OrderDetailComponent", "Muestra el detalle del pedido");
            
           //USES
           mobileApplication.Uses(accountWholeSaleController,"","");
           mobileApplication.Uses(payController,"","");
           mobileApplication.Uses(saleRecordSummaryController,"","");
           mobileApplication.Uses(accountRetailerController,"","");

           webApplication.Uses(accountWholeSaleController,"","");
           webApplication.Uses(payController,"","");
           webApplication.Uses(saleRecordSummaryController,"","");
           webApplication.Uses(accountRetailerController,"","");
           //
           saleRecordSummaryController.Uses(orderDetailComponent, "", "");
          //  uses
          accountWholeSaleController.Uses(modifyComponent, "", "");
          accountWholeSaleController.Uses(aproveComponent, "", "");
          accountWholeSaleController.Uses(trackingComponent, "", "");
          // uses
          accountRetailerController.Uses(orderSummaryComponent, "", "");
          
          //payController
          payController.Uses(securityComponent, "", "");
          payController.Uses(emailComponent, "", "");
          //others that use email
          orderDetailComponent.Uses(emailComponent, "", "");
           
          //uses database
          securityComponent.Uses(dataBase, "", "JDBC");
          orderDetailComponent.Uses(dataBase, "", "JDBC");
          
          //others
          emailComponent.Uses(gmail, "", "");
          securityComponent.Uses(payU, "", "");
          trackingComponent.Uses(googleCalendar, "", "");
          //tags
          
          emailComponent.AddTags("EmailComponent");
          orderSummaryComponent.AddTags("OrderSummaryComponent");
          accountRetailerController.AddTags("AccountRetailerController");
          modifyComponent.AddTags("ModifyComponent");
          aproveComponent.AddTags("AproveComponent");
          trackingComponent.AddTags("TrackingComponent");
          accountWholeSaleController.AddTags("AccountWholeSaleController");
          securityComponent.AddTags("SecurityComponent");
          payController.AddTags("PayController");
          saleRecordSummaryController.AddTags("SaleRecordSummaryController");
          orderDetailComponent.AddTags("OrderDetailComponent");

        //styles
        styles.Add(new ElementStyle("EmailComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("OrderSummaryComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AccountRetailerController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("ModifyComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AproveComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("TrackingComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("AccountWholeSaleController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("SecurityComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("PayController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("SaleRecordSummaryController") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });
        styles.Add(new ElementStyle("OrderDetailComponent") { Shape = Shape.Component,Color = "#ffffff", Background = "#555555", Icon = "" });


        ComponentView componentView = viewSet.CreateComponentView(apiRest, "Component", "Diagrama de componentes");
        componentView.PaperSize = PaperSize.A3_Landscape;
        componentView.AddAllElements();
        componentView.Remove(wholeSalerContext);
        componentView.Remove(retailerContext);
        componentView.Remove(notifyContext);
        componentView.Remove(paymentProcessContext);
        componentView.Remove(saleRecordContext);
            
            
            
            
            
            
            
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
            
            
        }
    }
}