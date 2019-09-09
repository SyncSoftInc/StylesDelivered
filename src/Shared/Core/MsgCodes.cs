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
        public const string ProductNotExists = nameof(ProductNotExists);
        public const string ASINCannotBeEmpty = nameof(ASINCannotBeEmpty);
        public const string ProductNameCannotBeEmpty = nameof(ProductNameCannotBeEmpty);
        public const string InvalidASINLength = nameof(InvalidASINLength);
        public const string InvalidProductNameLength = nameof(InvalidProductNameLength);
        public const string InvalidDescriptionLength = nameof(InvalidDescriptionLength);
        public const string InvalidImageUrlLength = nameof(InvalidImageUrlLength);

        //ProductItem
        public const string SKUCannotBeEmpty = nameof(SKUCannotBeEmpty);
        public const string InvalidSKULength = nameof(InvalidSKULength);
        public const string InvalidAliasLength = nameof(InvalidAliasLength);
        public const string InvalidColorLength = nameof(InvalidColorLength);
        public const string InvalidSizeLength = nameof(InvalidSizeLength);
        public const string InvalidInventoryQuantity = nameof(InvalidInventoryQuantity);
        // User
        public const string UserNotExists = nameof(UserNotExists);
        public const string AddressExists = nameof(AddressExists);
        public const string IDCannotBeEmpty = nameof(IDCannotBeEmpty);
        public const string UsernameCannotBeEmpty = nameof(UsernameCannotBeEmpty);
        public const string InvalidUsernameLength = nameof(InvalidUsernameLength);
        public const string InvalidPhoneLength = nameof(InvalidPhoneLength);
        public const string InvalidEmailLength = nameof(InvalidEmailLength);

        // Inventory
        public const string ShortageOfInventory = nameof(ShortageOfInventory);
    }
}
