using AutoFixture;
using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.ECP;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Review;
using SyncSoft.StylesDelivered.Domain.Review;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.DTO.Review;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Review
{
    public class Review
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IReviewService> _lazyReviewService = ObjectContainer.LazyResolve<IReviewService>();
        private IReviewService ReviewService => _lazyReviewService.Value;

        private static readonly Lazy<IReviewDAL> _lazyReviewDAL = ObjectContainer.LazyResolve<IReviewDAL>();
        private IReviewDAL ReviewDAL => _lazyReviewDAL.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        Fixture _fixture = new Fixture();

        private ClaimsIdentity _identity = new ClaimsIdentity(new Claim[] {
            new Claim(CONSTANTs.Claims.UserID, "10000001-1001-1001-1001-100000000001"),
            new Claim(ClaimTypes.Name, "sa@admin.com"),
            new Claim(ClaimTypes.Role, "4")
        });

        private ReviewDTO _review;
        private OrderDTO _order;

        #endregion
        // *******************************************************************************************************************************
        #region -  Setup & TearDown  -

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            _order = await OrderDAL.GetOrderAsync("e4b0270188944bed8e9bf7e3c1b8cad8").ConfigureAwait(false);
            _order.Items = await OrderItemDAL.GetOrderItemsAsync(_order.OrderNo).ConfigureAwait(false);

            // 构造Review
            _review = _fixture.Create<ReviewDTO>();
            _review.OrderNo = _order.OrderNo;
            _review.SKU = _order.Items.Select(x => x.SKU).FirstOrDefault();
            //_review.Title = "";
            _review.Content = "test_content!";
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        [Test]
        public async Task LifeCycle()
        {
            var createCmd = new CreateReviewCommand { Review = _review };
            createCmd.SetContext(_identity);
            var msgCode = await ReviewService.CreateReviewAsync(createCmd).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var list = await ReviewDAL.GetReviewsAsync(new SyncSoft.StylesDelivered.Query.Review.GetReviewsQuery { }).ConfigureAwait(false);
            var id = list.Items.OrderByDescending(x => x.CreatedOnUtc).Select(x => x.ID).FirstOrDefault();

            var approveCmd = new ApproveReviewCommand { ID = id };
            approveCmd.SetContext(_identity);
            msgCode = await ReviewService.ApproveReviewAsync(approveCmd).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var deleteCmd = new DeleteReviewCommand { ID = id };
            deleteCmd.SetContext(_identity);
            msgCode = await ReviewService.DeleteReviewAsync(deleteCmd).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
    }
}
