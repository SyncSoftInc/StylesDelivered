using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.Domain.Mail;
using SyncSoft.StylesDelivered.Domain.Order;
using SyncSoft.StylesDelivered.Domain.Product;
using SyncSoft.StylesDelivered.Domain.Review;
using SyncSoft.StylesDelivered.Domain.User;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStydDomain(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStydDomain));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IProductService, ProductService>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IProductItemService, ProductItemService>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IUserService, UserService>(LifeCycleEnum.Singleton);
                    //ObjectContainer.Register<IShoppingCartService, ShoppingCartService>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IItemInventoryFactory, ItemInventoryFactory>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IOrderService, OrderService>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IEmailSender, EmailSender>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IEmailService, EmailService>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IReviewService, ReviewService>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
