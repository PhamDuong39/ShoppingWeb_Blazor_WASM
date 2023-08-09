using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Xml.Linq;
using WASM.Client.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
   
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartViewModel> CartVMItems { get; set; }

        public string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected void NavigateToLogin()
        {
            NavigationManager.NavigateTo("/Login");
        }

       
        protected override async Task OnInitializedAsync()
        {
            try
            {
                CartVMItems = await this.ShoppingCartService.GetCartItems(FakeUserLogin.userId);
                // Call the function to Calculate the Summary Price of cart
                CartChanged();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private CartViewModel GetCartItem(int Id)
        {
            // Get the Item in the Cart By Id CartItem
            return CartVMItems.FirstOrDefault(p => p.Id == Id);
        }

        // Delete Item in the cart
        protected async Task DeleteCartItem_Click(int Id)
        {
            var cartItemVM = await this.ShoppingCartService.DeleteItem(Id);
            RemoveCartItem(Id);
            // Call the function to Calculate the Summary Price of cart
            CartChanged();
        }
      
        private void RemoveCartItem(int Id)
        {
            var cartItemVM = GetCartItem(Id);
            CartVMItems.Remove(cartItemVM);
        }

        // Change the number of product in the cart
        protected async Task UpdateQtyCartItem_Click(CartViewModel cartViewModel)
        {
            try
            {
                if(cartViewModel.Qty > 0)
                {
                    var updateItemVM = new CartViewModel
                    {
                        Id = cartViewModel.Id,
                        CartId = cartViewModel.CartId,
                        ProductId = cartViewModel.ProductId,
                        ProductName = cartViewModel.ProductName,
                        ProductDescription = cartViewModel.ProductDescription,
                        ProductImageURL = cartViewModel.ProductImageURL,
                        Price = cartViewModel.Price,
                        Qty = cartViewModel.Qty,
                    };
                    var returnedUpdateItemVM = await this.ShoppingCartService.UpdateQty(updateItemVM);
                    // Call the function to Update the TotalPrice after change
                    UpdateItemTotalPrice(returnedUpdateItemVM);
                    // Call the function to Calculate the Summary Price of cart : TotalQty, TotalPrice
                    CartChanged();
                }
                else
                {
                    var item = this.CartVMItems.FirstOrDefault(p => p.Id == cartViewModel.Id);
                    if(item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Set the Total Price
        private void SetTotalPrice()
        {
            // Get the sum Price
            TotalPrice = this.CartVMItems.Sum(p => p.TotalPrice).ToString("C");
        }

        private void SetToTalQuantity()
        {
            // Get the sum Quantity of each product
            TotalQuantity = this.CartVMItems.Sum(p => p.Qty);
        }

        private void UpdateItemTotalPrice(CartViewModel cartViewModel)
        {
            // Update the Total Price each time change the Item in the cart
            var item = GetCartItem(cartViewModel.Id);
            if(item != null)
            {
                item.TotalPrice = cartViewModel.Price * cartViewModel.Qty;
            }
        }

        private void CalculateCartSummaryTotals()
        {
            // Call this Func Wherever Change the number of Item in the cart
            SetTotalPrice();
            SetToTalQuantity();
        }

        //  Sự kiện OnShoppingCartChanged Sẽ được kích hoạt
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
