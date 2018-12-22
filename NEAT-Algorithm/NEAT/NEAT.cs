using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT
{
	public class Population
	{
		readonly HashSet<Tuple<int, int>> innovationNumbers;
		List<NeuralNetwork> population;

		public Population(IEnumerable<double> inputs, IEnumerable<string> outputNames, int size)
		{
			innovationNumbers = new HashSet<Tuple<int, int>>();
			population = new List<NeuralNetwork>();
			for (int i = 0; i < size; i++)
			{
				population.Add(new NeuralNetwork(inputs, outputNames, innovationNumbers));
			}
		}
	}

	public class NeuralNetwork
	{
		public Dictionary<int, Neuron> Neurons { get; private set; }
		public List<Tuple<int, int>> Connections {get; private set;}
		public int NumOutputs { get; private set; }
		public int NumInputs { get; private set; }
		public int NumNeurons { get; private set; }
		public HashSet<Tuple<int, int>> InnovationNumbers { get; private set; }
		List<string> outputNames;

		Random rand = new Random(Guid.NewGuid().GetHashCode());

		public NeuralNetwork(IEnumerable<double> inputs, IEnumerable<string> outputNames, HashSet<Tuple<int, int>> _innovationNumbers)
		{
			this.outputNames = (List<string>)outputNames;
			Neurons = new Dictionary<int, Neuron>();
			Connections = new List<Tuple<int, int>>();
			InnovationNumbers = _innovationNumbers;
			NumInputs = inputs.Count();
			NumOutputs = outputNames.Count();
			NumNeurons = NumInputs + NumOutputs;
			for (int id = 0; id < NumInputs; id++)
			{
				Neurons.Add(id, new Neuron(id, inputs.ElementAt(id)));
			}

			for (int id = 0; id < NumOutputs; id++)
			{
				Neurons.Add(id + NumInputs, new Neuron(id + NumInputs));
			}
		}

		private void InitializeRandom()
		{

		}

		//TODO: change to private
		public void MutateLink()
		{
			int from;
			int to;

			do
			{
				do
				{
					from = rand.Next(Neurons.Count());
				} while (from > NumInputs - 1 && from < NumInputs + NumOutputs);

				do
				{
					to = rand.Next(Neurons.Count());
				} while (to == from || to < NumInputs || (to > NumInputs + NumOutputs && to < from));
			} while (Connections.Contains(new Tuple<int, int>(from, to)));

			if (Neurons.TryGetValue(to, out Neuron toNeuron))
			{
				if (Neurons.TryGetValue(from, out Neuron fromNeuron))
				{
					Tuple<int, int> fromToPair = new Tuple<int, int>(from, to);
					Connection conn;
					if (InnovationNumbers.Contains(fromToPair))
					{
						conn = new Connection(fromToPair.GetHashCode(), fromNeuron, rand.NextDouble() * 4 - 2);
					}
					else
					{
						conn = new Connection(fromToPair.GetHashCode(), fromNeuron, rand.NextDouble() * 4 - 2);
						InnovationNumbers.Add(new Tuple<int, int>(from, to));
					}
					Connections.Add(fromToPair);
					toNeuron.AddConnection(conn.ID, conn);
				}
			}
		}

		public void MutateNeuron()
		{
			int neuronId = rand.Next(NumInputs, NumInputs + NumOutputs);
			if (Neurons.TryGetValue(neuronId, out Neuron farNeuron))
			{
				int connectionId = rand.Next(farNeuron.Connections.Count());
				Neuron newNeuron = new Neuron(NumNeurons);
				Connection conn = farNeuron.Connections.Values.ElementAt(connectionId);
				Tuple<int, int> nearToMidTuple = new Tuple<int, int>(conn.From.ID, newNeuron.ID);
				Connection nearToMid = new Connection(nearToMidTuple.GetHashCode(), conn.From, 1);

				Tuple<int, int> midToFarTuple = new Tuple<int, int>(newNeuron.ID, farNeuron.ID);
				Connection midtoFar = new Connection(midToFarTuple.GetHashCode(), newNeuron, conn.Weight);

				Neurons.Add(newNeuron.ID, newNeuron);
				newNeuron.AddConnection(nearToMid.ID, nearToMid);
				farNeuron.Connections.Remove(connectionId);
				farNeuron.AddConnection(midtoFar.ID, midtoFar);
				NumNeurons++;
			}
		}

		public override string ToString()
		{
			{
				StringBuilder sb = new StringBuilder();
				foreach (Neuron neuron in Neurons.Values)
				{
					foreach (Connection conn in neuron.Connections.Values)
					{
						sb.Append("(" + conn.From.ID + " -> " + neuron.ID + ", w=" + conn.Weight + ", inno="+conn.ID+")\n");
					}
				}
				return sb.ToString();
			}
		}

		public string generateDOT() {


			//subgraph cluster_level1{
			//	label = "Level 1";
			//	proc1[label = "{<f0> 1.0|<f1> One process here\n\n\n}" shape = Mrecord];
			//	proc2[label = "{<f0> 2.0|<f1> Other process here\n\n\n}" shape = Mrecord];
			//	store1[label = "<f0>    |<f1> Data store one"];
			//	store2[label = "<f0>   |<f1> Data store two"];
			//	{ rank = same; store1, store2}

			//}


			StringBuilder sb = new StringBuilder("digraph NeuralNetwork{\n");
			//for (int i = 0; i < NumOutputs; i++) {
			//	sb.Append(i+NumInputs+"[lable = ");
			//}
			sb.Append("subgraph cluster_level1{\nlabel = \"Inputs\";\n");
			for (int i = 0; i < NumInputs; i++) {
				Neuron neur = Neurons.Values.ElementAt(i);
				sb.Append(neur.ID+";\n");
			}
			sb.Append("}\nsubgraph cluster_level3{\nlabel = \"Outputs\";\n");
			for (int i = NumInputs; i < NumInputs+NumOutputs; i++)
			{
				Neuron neur = Neurons.Values.ElementAt(i);
				sb.Append(neur.ID + "[label=\""+outputNames.ElementAt(i-NumInputs)+"\"];\n");
			}
			sb.Append("}\nsubgraph cluster_level2{\nlabel = \"Hidden Layers\";\nrankdir=LR;\n");
			for (int i = NumInputs+NumOutputs; i <  Neurons.Count(); i++)
			{
				Neuron neur = Neurons.Values.ElementAt(i);
				sb.Append(neur.ID + ";\n");
			}
			sb.Append("}\n");
			foreach (Neuron neuron in Neurons.Values)
			{
				foreach (Connection conn in neuron.Connections.Values)
				{
					sb.Append(conn.From.ID + " -> " + neuron.ID + "[label=\"" + Math.Round(conn.Weight, 3) + "\"];\n");
				}
			}
			sb.Append("}");
			return sb.ToString();

			//foreach (Neuron neuron in Neurons.Values)
			//{
			//	foreach (Connection conn in neuron.Connections.Values)
			//	{
			//		sb.Append(conn.From.ID + " -> " + neuron.ID + "[label=\"" +conn.Weight+ "\"];\n");
			//	}
			//}
			//sb.Append("}");
		}

		//public static NeuralNetwork Crossover(NeuralNetwork NN1, NeuralNetwork NN2)
		//{
		//	Random rand = new Random(Guid.NewGuid().GetHashCode());
		//	NeuralNetwork childNetwork = new NeuralNetwork();

		//	foreach (Tuple<int, int> conn in NN1.) {

		//	}

		//	if (rand.NextDouble() >= .5) {
				
		//	}
		//}
	}

	public class Neuron
	{
		public int ID { get; private set; }
		public double Value { get; private set; }
		public Dictionary<int, Connection> Connections { get; private set; }

		public Neuron(int _id, double _value) : this(_id)
		{
			Value = _value;
		}

		public Neuron(int _id)
		{
			ID = _id;
			Connections = new Dictionary<int, Connection>();
		}

		public bool AddConnection(int id, Connection connection)
		{
			if (!Connections.ContainsKey(id))
			{
				Connections.Add(id, connection);
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class Connection
	{
		public bool Enabled { get; private set; }
		public int ID { get; private set; }
		public Neuron From { get; set; }
		public double Weight { get; private set; }

		Random rand = new Random(Guid.NewGuid().GetHashCode());

		public Connection(int _id, Neuron _from, double _weight)
		{
			ID = _id;
			From = _from;
			Weight = _weight;
			Enabled = true;
		}

		public void MutateWeightRandom()
		{
			Weight = rand.NextDouble() * 4 - 2;
		}

		public void MutateWeightShift()
		{
			double shift = rand.NextDouble() - 0.5;
			Weight += shift;
		}

		public void EnableDisable()
		{
			Enabled = !Enabled;
		}
	}
}
