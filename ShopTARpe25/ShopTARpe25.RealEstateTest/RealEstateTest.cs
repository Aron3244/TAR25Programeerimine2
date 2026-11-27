

using Microsoft.Identity.Client;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Act Siin tuleb kutsuda RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);
        }

        //teha test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().DetailAsync(databaseGuid);

            //Assert
            //vastupidist loogikat tuleb kasutada
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        [Fact]
        //mul on vaja testida DetailAsync meetotit
        //leiab ankeeti ülesse ehk õige id ostsib
        public async Task Should_GetByIdRealestate_WhenReturnEqual()
        {
            //See guid on juba andmebaasis olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            //kui vajutad nupule, siis see guid saadetakse andmebaasii, mida võrreldakse
            Guid guid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");


            await Svc<IRealEstateServices>().DetailAsync(guid);

            Assert.Equal(databaseGuid, guid);
        }

        //kustutamise meetotit on vaja testida. Andmebaasi on ankeet, mida
        //tahan ära kustutada

        [Fact]
        //vaja oleks mockida ankeeti ehk teha meetod, kus on nakeedi andmed
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            RealEstateDto dto = MockRealEstateData();
            


            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);
          

            //võrdlen id-sid, kas kustutsin sllle ära, mis olid andmebaasis

            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }

        //mul on kask ankeeti loodud ja ühe kustutan ära 
        //assertis võrdlen, kas need kask on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealestate()
        {
            //arrange
            var dto = MockRealEstateData();

            //act
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }


        //uupendab andmeid
        [Fact]
        public async Task Should_UpdateByIdRealEstate_WhenUpdateData()
        {
            Guid guid = new Guid("d3661e93-4701-40a7-9770-ef18cd3a3253");

            RealEstateDto dto = MockRealEstateData();
            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Updata location";
            domain.RoomNumber = 2;
            domain.BuildingType = "FLat";
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

           await Svc<IRealEstateServices>().Update(dto);

            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //DoseNotMatch tahab string andmentüüpi
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);


        }
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {
            //tuleb kasutada MockRealEstateDataUpdate ja MockRealEstateData2


            RealEstateDto dto = MockRealEstateDataUpdate();
            RealEstateDto dto2 = MockRealEstateData2();


           var CreateRealeEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(dto2);




            Assert.DoesNotMatch(CreateRealeEstate.Address, result.Address);
            Assert.NotEqual(CreateRealeEstate.ModifiedAt, result.ModifiedAt);
        }

        // kasutame ankeeti, kus kõik andmed on NULL
        //Tuleb kasutada create ja update 

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion3()
        {
            RealEstateDto dto = MockRealEstateData3();

            RealEstateDto update = MockRealEstateDataUpdate();


            var CreateRealeEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(update);

            Assert.NotEqual(CreateRealeEstate, result);

        }


        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAadress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now

            };
        }

        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area =120.00,
                Address = "SampleAadress",
                RoomNumber = 8,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now

            };
        }

        private RealEstateDto MockRealEstateData3()
        {
            return new RealEstateDto
            {
                Area = null,
                Address = null,
                RoomNumber = 0,
                BuildingType = null,
                CreatedAt =null,
                ModifiedAt = null

            };
        }

        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 100.00,
                Address = "WoodLand",
                RoomNumber = 2,
                BuildingType = "Mansion",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now

            };
        }

    }
}
