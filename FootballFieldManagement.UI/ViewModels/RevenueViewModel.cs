using FootballFieldManagement.Core.Repositories;
using FootballFieldManagement.Domain.Models;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.UI.ViewModels
{
    public class RevenueViewModel : BaseViewModel
    {
        private ObservableCollection<FieldType> _listFieldType;

        public ObservableCollection<FieldType> ListFieldType
        {
            get { return _listFieldType; }
            set { _listFieldType = value; OnPropertyChanged(); }
        }

        private DateTime _startDate = DateTime.Now;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); Caculator(); }
        }
        private DateTime _endDate = DateTime.Now;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); Caculator(); }
        }
        private string _fieldPrice;

        public string FieldPrice
        {
            get { return _fieldPrice; }
            set { _fieldPrice = value; OnPropertyChanged(); }
        }
        private string _productPrice;

        public string ProductPrice
        {
            get { return _productPrice; }
            set { _productPrice = value; OnPropertyChanged(); }
        }
        private string _total;

        public string Total
        {
            get { return _total; }
            set { _total = value; OnPropertyChanged(); }
        }
        private FieldType _selectedFieldType;

        public FieldType SelectedFieldType
        {
            get { return _selectedFieldType; }
            set { _selectedFieldType = value; OnPropertyChanged(); if (SelectedFieldType != null) Caculator(); }
        }

        public RevenueViewModel()
        {
            LoadCbo();
            Caculator();
        }
        public IRepository<FieldType> _fieldTypeRepository = new Repository<FieldType>(StaticClass.FootballFieldManagementDbContext);
        public IRepository<Bill> _billRepository = new Repository<Bill>(StaticClass.FootballFieldManagementDbContext);
        public void Caculator()
        {
            FieldPrice = _billRepository.AsQueryable()
                .Include(x => x.Field)
                .Where(x =>
                (SelectedFieldType == null || SelectedFieldType.Id == -1 || x.Field.FieldTypeId == SelectedFieldType.Id) &&
                (StartDate == null || x.DatePlay.Date >= StartDate.Date) &&
                (EndDate == null || x.DatePlay.Date <= EndDate.Date))
                .Sum(x => x.PriceField).ToString();
            FieldPrice = FormatCurrency(FieldPrice);
            ProductPrice = _billRepository.AsQueryable()
                .Include(x => x.Field)
                .Where(x =>
                (SelectedFieldType == null || SelectedFieldType.Id == -1 || x.Field.FieldTypeId == SelectedFieldType.Id) &&
                (StartDate == null || x.DatePlay.Date >= StartDate.Date) &&
                (EndDate == null || x.DatePlay.Date <= EndDate.Date))
                .Sum(x => x.PriceProduct).ToString();
            ProductPrice = FormatCurrency(ProductPrice);
            Total = _billRepository.AsQueryable()
                .Include(x => x.Field)
                .Where(x =>
                (SelectedFieldType == null || SelectedFieldType.Id == -1 || x.Field.FieldTypeId == SelectedFieldType.Id) &&
                (StartDate == null || x.DatePlay.Date >= StartDate.Date) &&
                (EndDate == null || x.DatePlay.Date <= EndDate.Date))
                .Sum(x => x.Total).ToString();
            Total = FormatCurrency(Total);

            if (!ListFieldType.Any(c => c.Id == -1 && c.Name == "Tất cả"))
            {
                ListFieldType.Insert(0, new FieldType { Id = -1, Name = "Tất cả" });
                OnPropertyChanged(nameof(ListFieldType));
            }
        }
        private void LoadCbo()
        {
            ListFieldType = new ObservableCollection<FieldType>(_fieldTypeRepository.AsQueryable().ToList());
        }
        public static string FormatCurrency(string numberString, string cultureName = null)
        {
            // Kiểm tra và chuyển đổi chuỗi thành số thập phân
            if (decimal.TryParse(numberString, out decimal number))
            {
                // Sử dụng văn hóa hiện tại nếu không cung cấp văn hóa cụ thể
                CultureInfo culture = cultureName == null ? CultureInfo.CurrentCulture : new CultureInfo(cultureName);

                // Định dạng số thành định dạng tiền tệ nhưng không có dấu thập phân
                string formattedNumber = number.ToString("N0", culture);

                // Trả về kết quả đã định dạng
                return formattedNumber;
            }
            else
            {
                // Trả về thông báo lỗi nếu chuỗi không hợp lệ
                return "Chuỗi không hợp lệ.";
            }
        }
    }
}
