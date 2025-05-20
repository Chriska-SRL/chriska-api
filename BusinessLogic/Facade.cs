using BusinessLogic.SubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Facade
    {
        private AuthSubSystem _auth  = new AuthSubSystem();

        private CategoriesSubSystem _category = new CategoriesSubSystem();

        private DeliveriesSubSystem _deliverie = new DeliveriesSubSystem();

        private  ClientsSubSystem _client = new ClientsSubSystem();

        private  OrdersSubSystem _order = new OrdersSubSystem();

        private  ProductsSubSystem _product = new ProductsSubSystem();

        private  PaymentsSubSystem _payment = new PaymentsSubSystem();

        private  StockSubSystem _stock = new StockSubSystem();

        private PurchasesSubSystem _purchase = new PurchasesSubSystem();

        private ReturnsSubSystem _return = new ReturnsSubSystem();

        private SuppliersSubSystem _supplier = new SuppliersSubSystem();

        private UserSubSystem _user = new UserSubSystem();

        private WarehousesSubSystem _warehouse = new WarehousesSubSystem();

        private ZonesSubSystem _zone = new ZonesSubSystem();


    }
}
