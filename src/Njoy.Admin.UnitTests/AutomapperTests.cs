using AutoMapper;
using Njoy.Data;
using Njoy.Services;
using Xunit;

namespace Njoy.Admin.UnitTests
{
    public sealed class AutomapperTests
    {
        [Fact]
        public void Can_Map_Partial()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CreateMerchantUserFeature.Request, CreateUserRequest>();
                c.CreateMap<Remaining, CreateUserRequest>();
            });
            IMapper mapper = new Mapper(config);

            var appRequest = new CreateMerchantUserFeature.Request
            {
                Username = "testusername",
                Password = "TestP@ssword",
                PasswordConfirm = "TestP@ssword",
                Email = "test@test.com",
                BusinessCode = "business123"
            };

            var mapped = mapper.Map<CreateUserRequest>(appRequest);

            Assert.Equal(appRequest.Username, mapped.Username);
            Assert.Equal(appRequest.Password, mapped.Password);
            Assert.Equal(appRequest.PasswordConfirm, mapped.PasswordConfirm);
            Assert.Equal(appRequest.Email, mapped.Email);

            var remaining = new Remaining
            {
                FirstName = "Test",
                LastName = "User",
                Role = AppRole.AdminStandard
            };

            mapper.Map(remaining, mapped);

            // First check if the mapper assigned incorrect values to incorrect object (which is the source)
            Assert.NotEmpty(remaining.FirstName);
            Assert.NotEmpty(remaining.LastName);
            Assert.NotEmpty(remaining.Role);

            // Check if the mapping results in the same values of related properties
            Assert.Equal(remaining.FirstName, mapped.FirstName);
            Assert.Equal(remaining.LastName, mapped.LastName);
            Assert.Equal(remaining.Role, mapped.Role);
        }

        [Fact]
        public void Can_Map_From_Subset_To_Superset()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<AppRequest, ServiceRequest>();
            });
            IMapper mapper = new Mapper(config);

            var subset = new AppRequest
            {
                Property1 = "testprop1",
                Property2 = 23,
                Property3 = "testprop2",
                Property4 = 37,
            };
            var extra = "extraprop";

            var mapped = mapper.Map<ServiceRequest>(subset);
            mapped.PropertyExtra = extra;

            Assert.Equal(subset.Property1, mapped.Property1);
            Assert.Equal(subset.Property2, mapped.Property2);
            Assert.Equal(subset.Property3, mapped.Property3);
            Assert.Equal(subset.Property4, mapped.Property4);
            Assert.Equal(extra, mapped.PropertyExtra);
        }

        public interface IModel1
        {
            string Property1 { get; set; }
            int Property2 { get; set; }
        }

        public interface IModel2
        {
            string Property3 { get; set; }
            int Property4 { get; set; }
        }

        public sealed class AppRequest : IModel1, IModel2
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public string Property3 { get; set; }
            public int Property4 { get; set; }
        }

        public sealed class ServiceRequest : IModel1, IModel2
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public string Property3 { get; set; }
            public int Property4 { get; set; }
            public string PropertyExtra { get; set; }
        }

        public sealed class Remaining
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Role { get; set; }
        }
    }
}