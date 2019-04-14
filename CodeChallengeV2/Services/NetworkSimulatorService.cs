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
            var gateways = network.Nodes.Where(node => node.Type.Equals("Gateway"));
            var devices = network.Nodes.Where(node => node.Type.Equals("Device"));

            var disconnected = devices.Where(node => getEdges(network, node.Id).Count() == 1);
            var failures = disconnected.Select(node => getEdges(network, node.Id).First().Target).ToList<string>();
            //var failureNodes = failures.Select(id => network.Nodes.FirstOrDefault(node => node.Id == id));

            var failureNodes = new List<Node>();

            foreach (var s in failures)
            {
                var gateway = gateways.FirstOrDefault(node => node.Id.Equals(s));

                if (gateway != null) {
                    failureNodes.Add(gateway);
                }
            }

            return Task.FromResult(failureNodes);
        }

        private List<Edge> getEdges(Graph graph, string id)
        {
            return graph.Edges.Where(edge => edge.Source.Equals(id)).ToList<Edge>();
        }
    }
}
