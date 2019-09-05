using SyncSoft.ECP;

namespace SyncSoft.StylesDelivered
{
    public static class MsgCodes
    {
        public const string SUCCESS = MSGCODEs.SUCCESS;

        public const string SaveFileToCloudFailed = nameof(SaveFileToCloudFailed);
        public const string DeleteFileOnCloudFailed = nameof(DeleteFileOnCloudFailed);
        public const string SecurityCheckFailed = nameof(SecurityCheckFailed);
        public const string CreateShoppingCartFailed = nameof(CreateShoppingCartFailed);

        // Product
        public const string ItemNoCannotBeEmpty = nameof(ItemNoCannotBeEmpty);
        public const string ProductNameCannotBeEmpty = nameof(ProductNameCannotBeEmpty);
        public const string InvalidItemNoLength = nameof(InvalidItemNoLength);
        public const string InvalidProductNameLength = nameof(InvalidProductNameLength);
        public const string InvalidDescriptionLength = nameof(InvalidDescriptionLength);
        public const string InvalidImageUrlLength = nameof(InvalidImageUrlLength);
        public const string InvalidInventoryQuantity = nameof(InvalidInventoryQuantity);

        // User
        public const string UserNotExists = nameof(UserNotExists);
        public const string AddressExists = nameof(AddressExists);
        public const string IDCannotBeEmpty = nameof(IDCannotBeEmpty);
        public const string InvalidPhoneLength = nameof(InvalidPhoneLength);
        public const string InvalidEmailLength = nameof(InvalidEmailLength);

        // Inventory
        public const string ShortageOfInventory = nameof(ShortageOfInventory);
    }
}
