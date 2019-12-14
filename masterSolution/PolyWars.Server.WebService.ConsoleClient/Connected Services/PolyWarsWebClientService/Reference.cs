﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolyWars.Server.WebService.ConsoleClient.PolyWarsWebClientService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PolyWarsWebClientService.IWebClientService")]
    public interface IWebClientService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/login", ReplyAction="http://tempuri.org/IWebClientService/loginResponse")]
        bool login(PolyWars.API.Network.Services.DataContracts.UserData userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/login", ReplyAction="http://tempuri.org/IWebClientService/loginResponse")]
        System.Threading.Tasks.Task<bool> loginAsync(PolyWars.API.Network.Services.DataContracts.UserData userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/register", ReplyAction="http://tempuri.org/IWebClientService/registerResponse")]
        bool register(PolyWars.API.Network.Services.DataContracts.UserData userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/register", ReplyAction="http://tempuri.org/IWebClientService/registerResponse")]
        System.Threading.Tasks.Task<bool> registerAsync(PolyWars.API.Network.Services.DataContracts.UserData userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/GetLeaderBoard", ReplyAction="http://tempuri.org/IWebClientService/GetLeaderBoardResponse")]
        PolyWars.API.Network.Services.DataContracts.LeaderboardEntryData[] GetLeaderBoard();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWebClientService/GetLeaderBoard", ReplyAction="http://tempuri.org/IWebClientService/GetLeaderBoardResponse")]
        System.Threading.Tasks.Task<PolyWars.API.Network.Services.DataContracts.LeaderboardEntryData[]> GetLeaderBoardAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWebClientServiceChannel : PolyWars.Server.WebService.ConsoleClient.PolyWarsWebClientService.IWebClientService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebClientServiceClient : System.ServiceModel.ClientBase<PolyWars.Server.WebService.ConsoleClient.PolyWarsWebClientService.IWebClientService>, PolyWars.Server.WebService.ConsoleClient.PolyWarsWebClientService.IWebClientService {
        
        public WebClientServiceClient() {
        }
        
        public WebClientServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebClientServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebClientServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebClientServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool login(PolyWars.API.Network.Services.DataContracts.UserData userData) {
            return base.Channel.login(userData);
        }
        
        public System.Threading.Tasks.Task<bool> loginAsync(PolyWars.API.Network.Services.DataContracts.UserData userData) {
            return base.Channel.loginAsync(userData);
        }
        
        public bool register(PolyWars.API.Network.Services.DataContracts.UserData userData) {
            return base.Channel.register(userData);
        }
        
        public System.Threading.Tasks.Task<bool> registerAsync(PolyWars.API.Network.Services.DataContracts.UserData userData) {
            return base.Channel.registerAsync(userData);
        }
        
        public PolyWars.API.Network.Services.DataContracts.LeaderboardEntryData[] GetLeaderBoard() {
            return base.Channel.GetLeaderBoard();
        }
        
        public System.Threading.Tasks.Task<PolyWars.API.Network.Services.DataContracts.LeaderboardEntryData[]> GetLeaderBoardAsync() {
            return base.Channel.GetLeaderBoardAsync();
        }
    }
}
