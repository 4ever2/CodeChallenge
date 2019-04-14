using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class NetworkSimulatorService : INetworkSimulatorService
    {
        /// <summary>
        /// Returns all nodes of type Gateway that if removed together with their connected edges would leave nodes of type Device, without edges to any Gateway. 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public Task<List<Node>> FindCritialGateways(NetworkGraph graph)
        {
            var network = graph.Graphs[0];
            //var failures = new List<Node>();

            var disconnected = network.Nodes.Where(node => node.Type == "Device" && getEdges(network, node.Id).Count() == 1);
            var failures = disconnected.Select(node => getEdges(network, node.Id).FirstOrDefault().Target);
            var failureNodes = failures.Select(id => network.Nodes.FirstOrDefault(node => node.Id == id));

            return Task.FromResult(failureNodes.ToList<Node>());
        }

        private List<Edge> getEdges(Graph graph, string id)
        {
            return graph.Edges.Where(edge => edge.Source == id).ToList<Edge>();
        }
    }
}
