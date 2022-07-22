using System.CommandLine;
using System.ServiceProcess;

namespace WinServicesConsole
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class Program
    {
        public enum SimpleServiceCustomCommands { StopWorker = 128, RestartWorker, CheckWorker };

        static int Main(string[] args)
        {

            var idOption = new Option<string>(
                name: "--id",
                description: "İşlem yapılacak servisin ID bilgisi."
            );

            var rootCommand = new RootCommand("Windows servis manipulasyon aracı");

            var listCommand = new Command(
                "list",
                "Varsayılan olarak aktif ve pasif olan tüm servisleri liste şeklinde yazdırır. false verilirse sadece aktifleri yazdırır, varsayılan true.");

            var detailCommand = new Command(
                "detail",
                "ID ile verilen servisin üzerinde hangi işlemlerin yapılabileceğini gösterir.")
                {
                    idOption
                };

            var stopServiceCommand = new Command(
                "stop",
                "ID ile verilen servisi durdurur.")
                {
                    idOption
                };

            var startForceServiceCommand = new Command(
            "start",
                "ID ile verilen servisi başlatır.")
                {
                    idOption
                };

            var restartForceServiceCommand = new Command(
            "restart",
                "ID ile verilen servisi yeniden başlatır.")
                {
                    idOption
                };


            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(detailCommand);
            rootCommand.AddCommand(stopServiceCommand);
            rootCommand.AddCommand(startForceServiceCommand);
            rootCommand.AddCommand(restartForceServiceCommand);

            listCommand.SetHandler(() => ListAllServices());
            detailCommand.SetHandler(async (id) => { await Detail(id); }, idOption);
            stopServiceCommand.SetHandler(async (id) => { await StopService(id); }, idOption);
            startForceServiceCommand.SetHandler(async (id) => { await StartService(id); }, idOption);
            restartForceServiceCommand.SetHandler(async (id) => { await RestartService(id); }, idOption);

            return rootCommand.InvokeAsync(args).Result;
        }

        static bool ListAllServices()
        {
            var onlyActiveServices = true;

            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            if (onlyActiveServices)
            {
                Console.WriteLine("ID : Name");

                foreach (ServiceController scTemp in scServices)
                {
                    Console.WriteLine("{0} : {1}", scTemp.ServiceName, scTemp.DisplayName);
                }

                return true;
            }

            Console.WriteLine("Status: ID : Name");

            foreach (ServiceController scTemp in scServices)
            {
                Console.WriteLine("{0} : {1} : {2}", scTemp.Status, scTemp.ServiceName, scTemp.DisplayName);
            }

            return true;
        }

        static async Task<bool> Detail(string name)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == name)
                {
                    ServiceController sc = new ServiceController(name);
                    Console.WriteLine("Status:                   " + sc.Status);
                    Console.WriteLine("Can Pause and Continue:   " + sc.CanPauseAndContinue);
                    Console.WriteLine("Can ShutDown:             " + sc.CanShutdown);
                    Console.WriteLine("Can Stop:                 " + sc.CanStop);
                    return true;
                }
            }

            return false;
        }

        static async Task<bool> StopService(string name)
        {
            Console.WriteLine("Durdurulmaya çalışılacak servisin ismi: {0}", name);

            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == name)
                {
                    ServiceController sc = new ServiceController(name);
                    if (!sc.CanStop)
                    {
                        Console.WriteLine("Bu servis durdurulamaz.");
                        return false;
                    }

                    sc.Stop();
                    // sc.ExecuteCommand((int)SimpleServiceCustomCommands.StopWorker);

                    Thread.Sleep(1000);

                    Console.WriteLine("{0} servisi başarıyla durdurldu. Güncel status: {1}", name, sc.Status);
                    return true;
                }
            }

            Console.WriteLine("{0} servisi bulunamadı.", name);
            return false;
        }

        static async Task<bool> StartService(string name)
        {
            Console.WriteLine("Başlatılmaya çalışılacak servisin ismi: {0}", name);

            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == name)
                {
                    ServiceController sc = new ServiceController(name);

                    sc.ExecuteCommand((int)SimpleServiceCustomCommands.RestartWorker);
                    Console.WriteLine("{0} servisi başarıyla başlatıldı. Güncel status: {1}", name, sc.Status);
                    return true;
                }
            }

            Console.WriteLine("{0} servisi bulunamadı.", name);
            return false;
        }

        static async Task<bool> RestartService(string name)
        {
            Console.WriteLine("Yeniden başlatılmaya çalışılacak servisin ismi: {0}", name);

            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == name)
                {
                    ServiceController sc = new ServiceController(name);

                    sc.ExecuteCommand((int)SimpleServiceCustomCommands.RestartWorker);
                    Console.WriteLine("{0} servisi başarıyla yeniden başlatıldı. Güncel status: {1}", name, sc.Status);
                    return true;
                }
            }

            Console.WriteLine("{0} servisi bulunamadı.", name);
            return false;
        }

    }
}
