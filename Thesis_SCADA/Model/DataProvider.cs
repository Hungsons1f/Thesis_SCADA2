using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis_SCADA.Model
{
    public class DataProvider
    {
        //Cách để tạo duy nhất một đối tượng DataProvider, từ đó tạo duy nhất một đối tượng DB
        //Cách này thay cho dùng static class

        //Luồng thực thi: 1) biến mới tạo được gán giá trị bằng _ins, vì _ins mang đối tượng DataProvider
        //2) Từ DataProvider.Ins khởi tạo DB, sau đó trả về cho biến mới tạo
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null) _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public ScadaEntities DB { get; set; }

        //Đặt pt khởi tạo là private để không thể tạo đối tượng bằng lớp này từ bên ngoài
        private DataProvider()
        {
            DB = new ScadaEntities();
        }
    }
}
