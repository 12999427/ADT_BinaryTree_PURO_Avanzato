using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace ADT_BinaryTree_Puro
{
    class BinaryTree<T>
    {
        public class Node<TT> //evitare di nascondere il parametro
        {
            public Node<TT>? LeftNode { get; set; }
            public Node<TT>? RightNode { get; set; }
            public TT Value { get; set;  }

            public Node(TT data)
            {
                this.Value = data;
                LeftNode = null;
                RightNode = null;
            }

            public int Compare(Node<TT>? other)
            {
                if (other != null)
                    return Comparer<TT>.Default.Compare(Value, other.Value);
                return -2;
            }
        }

        public Node<T>? Root { get; private set; }
        
        public BinaryTree()
        {
            this.Root = null;
        }

        public BinaryTree(Node<T> r)
        {
            this.Root = r;
        }

        public BinaryTree(T value) : this()
        {
            InsertLeaf(value);
        }

        public BinaryTree(IEnumerable<T> list) : this()
        {
            var a = list.ToArray();
            Root = generateFullBinaryTree(a);
        }

        #region INSERIMENTO VALORE

        //Genera albero da un'array di valori - lo si può fare solo all'inizio altrimenti l'albero non sarebbe completo
        private Node<T>? generateFullBinaryTree(T[] arr, int location = 0)
        {
            if (location >= arr.Length)
            {
                return null;
            }

            var node = new Node<T>(arr[location]);
            node.LeftNode = generateFullBinaryTree(arr, (location * 2) + 1);
            node.RightNode = generateFullBinaryTree(arr, (location * 2) + 2);

            return node;
        }

        //Aggiunge un nodo come ultimo (mantenendo l'albero "completo")
        public Node<T> InsertLeaf(T value)
        {
            return InsertLeafFromParent(value, Root!);
        } 
        public Node<T> InsertLeafFromParent(T value, Node<T> parent) 
        {
            var newNode = new Node<T>(value);

            if (Root == null)
            {
                Root = newNode;
            }
            else
            {
                Queue<Node<T>> queue = new Queue<Node<T>>();
                queue.Enqueue(parent);

                while (queue.Count > 0){
                    var current = queue.Dequeue();

                    if (current.LeftNode == null)
                    {
                        current.LeftNode = newNode;
                        return newNode;
                    }
                    queue.Enqueue(current.LeftNode);

                    if (current.RightNode == null)
                    {
                        current.RightNode = newNode;
                        return newNode;
                    }
                    queue.Enqueue(current.RightNode);
                }
            }
            return newNode;
        }

        public Node<T> InsertSubtree(Node<T> parent, BinaryTree<T> tree)
        {
            if (tree.Root == null) throw new Exception();

            if(parent.LeftNode == null)
            {
                parent.LeftNode = tree.Root;
            }
            else if (parent.RightNode == null)
            {
                parent.RightNode = tree.Root;
            }
            else throw new ArgumentException("parent has no space for new children");

            return tree.Root;
        }

        public BinaryTree<T> RemoveSubtree(Node<T> parent)
        {
            if (parent == null) throw new Exception();

            var parents_parent = Padre_FindParent(parent);
            if (parents_parent == null) //parent is Root
            { 
                Root = null;
            }
            else if (parents_parent.LeftNode == parent)
            {
                parents_parent.LeftNode = null;
            }
            else if (parents_parent.RightNode == parent)
            {
                parents_parent.RightNode = null;
            }
            return new BinaryTree<T>(parent);
        }

        #endregion

        #region RIMOZIONE VALORE

        public T Peek()
        {
            Node<T>? last = FindLastNode();
            if (last == null)
                throw new IndexOutOfRangeException();
            
            return last.Value;
        }

        public T Pop()
        {
            if (Root == null)
            {
                throw new IndexOutOfRangeException();
            }

            var res = FindLastNodeRecursive(Root);
            if (res.node_parent == null) //root
            {
                Root = null;
            }
            else if (res.node_index % 2 == 1)
            {
                res.node_parent.LeftNode = null;
            }
            else
            {
                res.node_parent.RightNode = null;
            }
            return res.node!.Value;
        }

        private Node<T>? FindLastNode()
        {
            if (Root == null)
            {
                return null; //nullo
            }

            return FindLastNodeRecursive(Root).Item1!;
        }

        private (Node<T>? node, Node<T>? node_parent, int node_index) FindLastNodeRecursive(Node<T>? current, Node<T>? parent=null, int index=0)
        {
            if (current == null)
            {
                return (null, parent, -1);
            }
            else
            {
                Node<T>? last_from_left;
                Node<T>? last_from_left_p;
                int last_from_left_index;

                Node<T>? last_from_right;
                Node<T>? last_from_right_p;
                int last_from_right_index;

                (last_from_left, last_from_left_p, last_from_left_index) = FindLastNodeRecursive(current.LeftNode, current, index*2 +1);
                (last_from_right, last_from_right_p, last_from_right_index) = FindLastNodeRecursive(current.RightNode, current, index*2 +2);

                if (last_from_left_index == last_from_right_index && last_from_left_index == -1)
                {
                    return (current, parent, index);
                }
                else if (last_from_left_index > last_from_right_index)
                {
                    return (last_from_left, last_from_left_p, last_from_left_index);
                }
                else
                {
                    return (last_from_right, last_from_right_p, last_from_right_index);
                }
            }
        }

        #endregion
       
        // Stampa tutto l'albero (in orizzontale)
        private void PrintFromNode(Node<T>? node, int level)
        {
            if (node == null) return;

            PrintFromNode(node.RightNode, level + 1);

            Console.WriteLine(new string(' ', level * 4) + node.Value);

            PrintFromNode(node.LeftNode, level + 1);
        }

        public void Print(){
            PrintFromNode(Root, 0);
        } 

        private int NodesCountFromSomeNode(Node<T>? node)
        {
            return node == null ? 0 : NodesCountFromSomeNode(node.LeftNode) + NodesCountFromSomeNode(node.RightNode) + 1;
        }

        public int NodesCount {get{ return NodesCountFromSomeNode(Root);} }

        public int Grado_NumberAllChildrenOfSomeNode (Node<T> node)
        {
            return node == null ? 0 : NodesCountFromSomeNode(node.LeftNode) + NodesCountFromSomeNode(node.RightNode);
        }


        private int GetTreeDepthFromSomeNode(Node<T>? node)  //profondita'
        {
            return node == null ? 0 : Math.Max(GetTreeDepthFromSomeNode(node.LeftNode), GetTreeDepthFromSomeNode(node.RightNode)) + 1;
        }

        public int TreeDepth { get { return GetTreeDepthFromSomeNode(Root); } }

        public int NumImmediateChildrenOfSomeNode(Node<T> node)
        {
            int nfigli = 0;
            if (node.LeftNode != null)
                nfigli++;
            if (node.RightNode != null)
                nfigli++;
            return nfigli;
        }

        public List<Node<T>> GetImmediateChildNodesFromSomeNode (Node<T> node)
        {
            var r = new List<Node<T>>();
            if (node.LeftNode != null) r.Add(node.LeftNode);
            if (node.RightNode != null) r.Add(node.RightNode);
            return r;
        }

        public List<Node<T>> Figli_GetAllChildNodesFromSomeNode (Node<T>? node)
        {
            var r = new List<Node<T>>();
            GetAllChildNodesFromSomeNodeRecursive(node, r);
            return r;
        }

        private void GetAllChildNodesFromSomeNodeRecursive (Node<T>? node, List<Node<T>> buffer)
        {
            if (node != null)
            {
                buffer.Add(node);
                GetAllChildNodesFromSomeNodeRecursive(node.RightNode, buffer);
                GetAllChildNodesFromSomeNodeRecursive(node.LeftNode, buffer);
            }
        }

        public Node<T>? Padre_FindParent(Node<T> node)
        {
            if (node == this.Root)
            {
                return null;
            }

            return RecursiveFindParent(this.Root, node, null);
        }

        private Node<T>? RecursiveFindParent(Node<T>? current, Node<T> RealNode, Node<T>? parent)
        {
            if (current == null)
                return null; // CASO BASE: sono arrivato sulla foglia


            if (current.Compare(RealNode) == 0)
                return parent; // Abbiamo trovato il nodo, restituisci il suo genitore


            // Continua la ricerca nei sottoalberi
            Node<T>? leftResult = RecursiveFindParent(current.LeftNode, RealNode, current);
            if (leftResult != null)
            {
                return leftResult; // Se trovo il nodo nel sottoalbero sinistro, restituisco il suo genitore
            }

            // Se non lo trovo nel sottoalbero sinistro, cercho nel sottoalbero destro
            return RecursiveFindParent(current.RightNode, RealNode, current);
        }



  }
}