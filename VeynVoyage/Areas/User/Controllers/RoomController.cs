using System.Net;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeynVoyage.Areas.User.ViewModels;
using VeynVoyage.Data;
using VeynVoyage.Services.Interfaces;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using VeynVoyage.Models;

namespace VeynVoyage.Areas.User.Controllers
{
	[Area("User")]
	public class RoomController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IRoomService _roomService;
		public RoomController(IRoomService roomService, ApplicationDbContext context)
		{
			_roomService = roomService;
			_context = context;
		}

		public IActionResult Index()
		{
			var rooms = _roomService.GetRooms();
			return View(rooms);

		}


		//Rezervasyon

		public IActionResult Reserve(int id)
		{
			// Odayı bul
			var room = _roomService.GetRoomById(id);
			if (room == null)
			{
				// Oda bulunamadıysa hata döndür
				return NotFound();
			}

			// Rezervasyon view modelini oluştur
			var reservationViewModel = new ReservationViewModel
			{
				RoomId = room.Id,
				RoomName = room.Name,
				Price = room.Price,
				ImageUrl = room.Image,
				CheckInDate = DateTime.Today,
				CheckOutDate = DateTime.Today.AddDays(1)
			};

			// Rezervasyon detay sayfasını göster
			return View("Reserve", reservationViewModel);
		}

		[HttpPost]
		public IActionResult Reserve(DateTime checkinDate, DateTime checkoutDate, int roomId, int roomPricePerDay)
		{
			var room = _roomService.GetRoomById(roomId);
			if (room == null)
			{
				return NotFound();
			}

			int totalDays = (checkoutDate - checkinDate).Days;
			if (totalDays <= 0)
			{
				// Hata mesajı göster
			
				return View();
			}
			decimal totalPrice = totalDays * roomPricePerDay;

			HttpContext.Session.SetString("roomID", roomId.ToString());
			HttpContext.Session.SetString("totalPayment", totalPrice.ToString());

			return RedirectToAction("Order", "Room");
			

		}
		public IActionResult Order()
		{
			var roomID = HttpContext.Session.GetString("roomID");
			var totalPayment = HttpContext.Session.GetString("totalPayment");

	

			Options options = new Options(); // Iyzico Import
			options.ApiKey = "sandbox-i2QGNEjIjrWpC7l08BSHxqMCrFNcDj54";
			options.SecretKey = "sandbox-PDG8EnklAYWi9zYdjjTXKzr7Mu0Evtfu";
			options.BaseUrl = "Https://sandbox-api.iyzipay.com";

			

			CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
			request.Locale = Locale.TR.ToString();
			request.ConversationId = "123456789";
			request.Price = totalPayment; ;
			request.PaidPrice = totalPayment; ;
			request.Currency = Currency.TRY.ToString();
			request.BasketId = "B67832";
			request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
			request.CallbackUrl = "https://localhost:7247/User/Room/Success/Index";

			

			Buyer buyer = new Buyer();
			buyer.Id = "asdadsada";
			buyer.Name = "Erhan";
			buyer.Surname = "Kaya";
			buyer.GsmNumber = "+905554443322";
			buyer.Email = "email@email.com";
			buyer.IdentityNumber = "74300864791";
			buyer.LastLoginDate = "2015-10-05 12:43:35";
			buyer.RegistrationDate = "2000-12-12 12:00:00";
			buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
			buyer.Ip = "85.34.78.112";
			buyer.City = "Istanbul";
			buyer.Country = "Turkey";
			buyer.ZipCode = "34732";
			request.Buyer = buyer;

			Address shippingAddress = new Address();
			shippingAddress.ContactName = "Emrullah Yabasun";
			shippingAddress.City = "Istanbul";
			shippingAddress.Country = "Turkey";
			shippingAddress.Description = "Bereket döner karşısı";
			shippingAddress.ZipCode = "34742";
			request.ShippingAddress = shippingAddress;

			Address billingAddress = new Address();
			billingAddress.ContactName = "Erhan Kaya";
			billingAddress.City = "Istanbul";
			billingAddress.Country = "Turkey";
			billingAddress.Description = "Bereket Döner";
			billingAddress.ZipCode = "34742";
			request.BillingAddress = billingAddress;

			List<BasketItem> basketItems = new List<BasketItem>();
			BasketItem basketProduct;
			basketProduct = new BasketItem();
			basketProduct.Id = "1";
			basketProduct.Name = "Asus Bilgisayar";
			basketProduct.Category1 = "Bilgisayar";
			basketProduct.Category2 = "";
			basketProduct.ItemType = BasketItemType.PHYSICAL.ToString();

			double price = Convert.ToDouble(totalPayment);
			double endPrice = Convert.ToDouble(totalPayment);
			basketProduct.Price = endPrice.ToString().Replace(",", "");
			basketItems.Add(basketProduct);

			request.BasketItems = basketItems;

			CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
			ViewBag.pay = checkoutFormInitialize.CheckoutFormContent;
			return View();
		}
	
		
		
		public IActionResult Success()
		{
			return View();
		}
	}

}
