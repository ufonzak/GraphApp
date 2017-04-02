﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegrationTests.Services {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GraphNode", Namespace="http://schemas.datacontract.org/2004/07/WebServices.DAO")]
    [System.SerializableAttribute()]
    public partial class GraphNode : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] AdjacentNodeIDsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LabelField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] AdjacentNodeIDs {
            get {
                return this.AdjacentNodeIDsField;
            }
            set {
                if ((object.ReferenceEquals(this.AdjacentNodeIDsField, value) != true)) {
                    this.AdjacentNodeIDsField = value;
                    this.RaisePropertyChanged("AdjacentNodeIDs");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Label {
            get {
                return this.LabelField;
            }
            set {
                if ((object.ReferenceEquals(this.LabelField, value) != true)) {
                    this.LabelField = value;
                    this.RaisePropertyChanged("Label");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.IGraphManagementService")]
    public interface IGraphManagementService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphManagementService/GetGraphNode", ReplyAction="http://tempuri.org/IGraphManagementService/GetGraphNodeResponse")]
        IntegrationTests.Services.GraphNode GetGraphNode(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphManagementService/GetGraphNode", ReplyAction="http://tempuri.org/IGraphManagementService/GetGraphNodeResponse")]
        System.Threading.Tasks.Task<IntegrationTests.Services.GraphNode> GetGraphNodeAsync(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphManagementService/SyncGraphNode", ReplyAction="http://tempuri.org/IGraphManagementService/SyncGraphNodeResponse")]
        void SyncGraphNode(IntegrationTests.Services.GraphNode node);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGraphManagementService/SyncGraphNode", ReplyAction="http://tempuri.org/IGraphManagementService/SyncGraphNodeResponse")]
        System.Threading.Tasks.Task SyncGraphNodeAsync(IntegrationTests.Services.GraphNode node);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGraphManagementServiceChannel : IntegrationTests.Services.IGraphManagementService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GraphManagementServiceClient : System.ServiceModel.ClientBase<IntegrationTests.Services.IGraphManagementService>, IntegrationTests.Services.IGraphManagementService {
        
        public GraphManagementServiceClient() {
        }
        
        public GraphManagementServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GraphManagementServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GraphManagementServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GraphManagementServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public IntegrationTests.Services.GraphNode GetGraphNode(string id) {
            return base.Channel.GetGraphNode(id);
        }
        
        public System.Threading.Tasks.Task<IntegrationTests.Services.GraphNode> GetGraphNodeAsync(string id) {
            return base.Channel.GetGraphNodeAsync(id);
        }
        
        public void SyncGraphNode(IntegrationTests.Services.GraphNode node) {
            base.Channel.SyncGraphNode(node);
        }
        
        public System.Threading.Tasks.Task SyncGraphNodeAsync(IntegrationTests.Services.GraphNode node) {
            return base.Channel.SyncGraphNodeAsync(node);
        }
    }
}
