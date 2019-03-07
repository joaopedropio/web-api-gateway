using System;
using System.Threading.Tasks;
using WebAPIGateway.JSON;
using static System.Net.HttpStatusCode;

namespace WebAPIGateway.Domain
{
    public class ServiceActions : IServiceActions
    {
        private IServiceRepository serviceRepo;
        public ServiceActions(IServiceRepository serviceRepo)
        {
            this.serviceRepo = serviceRepo;
        }

        public async Task<IJsonResponse> GetService(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return new JsonResponse(new ErrorJsonData(Messages.ServiceNotProvided), BadRequest);
            }

            IService service;
            try
            {
                service = await serviceRepo.RetrieveAsync(serviceName);
            }
            catch (Exception ex)
            {
                return new JsonResponse(new ErrorJsonData(ex.Message), InternalServerError);
            }

            if (string.IsNullOrEmpty(service?.URL))
            {
                return new JsonResponse(new ErrorJsonData(Messages.ServiceNotFound),  NotFound);
            }

            return new JsonResponse(service, OK);
        }

        public async Task<IJsonResponse> PostService(IService service)
        {
            if (string.IsNullOrEmpty(service?.Name))
                return new JsonResponse(new ErrorJsonData(Messages.ServiceNameCantBeEmpty), BadRequest);

            if (string.IsNullOrEmpty(service?.URL))
                return new JsonResponse(new ErrorJsonData(Messages.ServiceURLCantBeEmpty), BadRequest);

            try
            {
                await serviceRepo.StoreAsync(new Service(service.Name, service.URL));
            }
            catch (Exception ex)
            {
                return new JsonResponse(new ErrorJsonData(ex.Message), InternalServerError);
            }

            return new JsonResponse(new SuccessJsonData(Messages.ServiceAdded), Created);
        }
        
        public async Task<IJsonResponse> DeleteService(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return new JsonResponse(new ErrorJsonData(Messages.ServiceNotProvided), BadRequest);
            }

            IService service;
            try
            {
                service = await serviceRepo.RetrieveAsync(serviceName);

                if (string.IsNullOrEmpty(service?.URL))
                    return new JsonResponse(new ErrorJsonData(Messages.ServiceNotFound), NotFound);

                await serviceRepo.RemoveAsync(serviceName);
            }
            catch (Exception ex)
            {
                return new JsonResponse(new ErrorJsonData(ex.Message), InternalServerError);
            }

            return new JsonResponse(new SuccessJsonData(Messages.ServiceDeleted), NoContent);
        }
    }
}
