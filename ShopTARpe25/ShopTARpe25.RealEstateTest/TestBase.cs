using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.RealEstateTest.Macros;





//Näitab kus asub
namespace ShopTARpe25.RealEstateTest
{
    // Abstraktne põhiklass testidele
    public abstract class TestBase

    {      
      // Muutuja, mis hoiab endas kõiki registreeritud teenuseid ja andmebaasi
        protected IServiceProvider ServiceProvider { get; set; }


        // Konstruktor – käivitub automaatselt iga kord, kui uus test algab
        protected TestBase()
        {
            // Loob uue tühja nimekirja teenuste testimiskes
            var services = new ServiceCollection();

            // Kutsub välja allpool oleva meetodi, mis täidab selle nimekirja teenuste ja andmebaasiga
            SetupServices(services);

            // Ehitab nimekirja põhjal valmis teenusepakkuja, kust testid saavad hiljem vajalikke osi küsida
            ServiceProvider = services.BuildServiceProvider();
        }

        // Meetod, kus pannakse kirja, milliseid teenuseid ja andmebaasi testides kasutatakse
        public virtual void SetupServices(IServiceCollection services)
        {
            // Ütleb programmile: kui keegi küsib IRealEstateServices liidest, anna talle RealEstateServices klass
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            // Seadistab andmebaasi konteksti (ShopTARpe25Context) testide jaoks
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                // Käseb päris andmebaasi asemel kasutada ajutist mälupõhist (InMemory) andmebaasi nimega "Test"
                x.UseInMemoryDatabase("Test");

                // Eirab hoiatusi, mis tekivad siis, kui kood üritab mälubaasis tehinguid (Transactions) kasutada
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            // Registreerib automaatselt kõik testides kasutatavad abiklassid (makrod)
            RegisterMacros(services);
        }

        // Tühi meetod ressursside vabastamiseks pärast testi lõppu (vajadusel saab siia puhastuskoodi lisada)
        public void Dispose()
        {

        }

        // Abimeetod, millega saab testides lühidalt küsida ükskõik millist registreeritud teenust (nt Svc<IRealEstateServices>())
        protected T Svc<T>()
        {
            // Otsib teenusepakkujast küsitud tüüpi teenuse ja tagastab selle
            return ServiceProvider.GetService<T>();
        }

        // Privaatne abimeetod, mis leiab automaatselt üles kõik makrod ja lisab need teenuste hulka
        private void RegisterMacros(IServiceCollection services)
        {        
            // Võtab aluseks IMacros liidese tüübi, et teada, milliseid klasse otsida
            var macroBaseType = typeof(IMacros);

            // Otsib projektist üles kõik klassid, mis kasutavad IMacros liidest (ja pole ise liidesed ega abstraktsed)
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
               && !t.IsInterface && !t.IsAbstract);

            // Käib kõik leitud makro-klassid ükshaaval läbi
            foreach (var macro in macros)
            {
                // Lisab makro teenuste hulka nii, et see luuakse valmis ainult ühe korra. 
                // Iga kord, kui test seda makrot küsib, antakse talle sama juba olemasolev objekt, mitte ei tehta uut.

                services.AddSingleton(macro);
            }
        }
    }
}
