using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT
{
	public class Population
	{
		Dictionary<int, Tuple<int, int>> innovationNumbers;
		List<NeuralNetwork> population;

		public Population(IEnumerable<double> inputs, IEnumerable<string> outputNames, int size)
		{
			innovationNumbers = new Dictionary<int, Tuple<int, int>>();
			population = new List<NeuralNetwork>();
			for (int i = 0; i < size; i++)
			{
				population.Add(new NeuralNetwork(inputs, outputNames, innovationNumbers));
			}
		}
	}

	public class NeuralNetwork
	{
		private Dictionary<int, Neuron> neurons;
		public int numOutputs { get; private set; }
		public int numInputs { get; private set; }
		public int numNeurons { get; private set; }
		public int numLinks { get; private set; }
		private Dictionary<int, Tuple<int, int>> innovationNumbers;

		Random rand = new Random();

		public NeuralNetwork(IEnumerable<double> inputs, IEnumerable<string> outputNames, Dictionary<int, Tuple<int, int>> _innovationNumbers)
		{
			numLinks = 0;
			neurons = new Dictionary<int, Neuron>();
			numInputs = inputs.Count();
			numOutputs = outputNames.Count();
			numNeurons = numInputs + numOutputs;
			for (int id = 0; id < numInputs; id++)
			{
				neurons.Add(id, new Neuron(id, inputs.ElementAt(id)));
			}

			for (int id = 0; id < numOutputs; id++)
			{
				neurons.Add(id + numInputs, new Neuron(id + numInputs));
			}

			InitializeRandom();
		}

		private void InitializeRandom()
		{

		}

		//TODO: change to private
		public bool mutateLink()
		{
			int from;
			int to;

			do
			{
				from = rand.Next(neurons.Count());
			} while (from > numInputs-1 && from < numInputs + numOutputs);

			do
			{
				to = rand.Next(neurons.Count());
			} while (to < numInputs || (to > numInputs + numOutputs && to < from));

			if (neurons.TryGetValue(to, out Neuron toNeuron))
			{
				if (neurons.TryGetValue(to, out Neuron fromNeuron))
				{
					Connection conn = new Connection(numLinks, fromNeuron, rand.NextDouble() * 4 - 2);
					toNeuron.addConnection(numLinks, conn);
					Console.WriteLine(from + " to " + to);
					numLinks++;
				}
			}

			return true;
		}

		public bool mutateNeuron()
		{
			int neuronId = rand.Next(numInputs, numInputs + numOutputs);
			Console.WriteLine(neuronId);
			if (neurons.TryGetValue(neuronId, out Neuron farNeuron)) {
				int connectionIdx = rand.Next(farNeuron.connections.Count());
				Neuron newNeuron = new Neuron(numNeurons-1);
				if (farNeuron.connections.TryGetValue(neuronId, out Connection conn))
				{

					Connection nearToMid = new Connection(numLinks, conn.From, 1);
					conn.From = newNeuron;
				}
				else
				{
					return false;
				}
			}
			return true;
		}
	}

	class Neuron
	{
		public int ID { get; private set; }
		public double Value { get; private set; }
		public Dictionary<int, Connection> connections { get; private set; }

		public Neuron(int _id, double _value) : this(_id)
		{
			Value = _value;
		}

		public Neuron(int _id)
		{
			ID = _id;
			connections = new Dictionary<int, Connection>();
		}

		public bool addConnection(int id, Connection connection)
		{
			if (!connections.ContainsKey(id))
			{
				connections.Add(id, connection);
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	class Connection
	{
		public bool Enabled { get; private set; }
		public int Id { get; private set; }
		public Neuron From { get; set; }
		public double Weight { get; private set; }

		Random rand = new Random();

		public Connection(int _id, Neuron _from, double _weight)
		{
			Id = _id;
			From = _from;
			Weight = _weight;
			Enabled = true;
		}

		public void mutateWeightRandom()
		{
			Weight = rand.NextDouble() * 4 - 2;
		}

		public void mutateWeightShift()
		{
			double shift = rand.NextDouble() - 0.5;
			Weight += shift;
		}

		public void enable_disable()
		{
			Enabled = !Enabled;
		}
	}
}
