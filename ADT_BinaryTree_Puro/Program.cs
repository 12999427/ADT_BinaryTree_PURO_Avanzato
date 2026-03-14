using System.Diagnostics;

namespace ADT_BinaryTree_Puro; //BINARY TREE CORRETTO
// NOTA: Come altro albero c'è l'implementazion della taffurelli caricata su classroom
class Program
{
    static void Main(string[] args)
    {
        BinaryTree<int> binaryTree = new();
        
        for (int i = 0; i<17; i++)
        {
            binaryTree.InsertLeaf(i);
        }

        binaryTree.Print();
        
        for (int i = 0; i<1; i++)
        {
            Console.WriteLine("\n________________\nValue " + binaryTree.Pop() + "\n");
            binaryTree.Print();
        }

        //Console.WriteLine(binaryTree.Peek());

        var f = binaryTree.Figli_GetAllChildNodesFromSomeNode(binaryTree.Root);
        var r_c = binaryTree.GetImmediateChildNodesFromSomeNode(binaryTree.Root)[1];
        var l_c = binaryTree.Figli_GetAllChildNodesFromSomeNode(r_c)[^4];
        
        Console.WriteLine(">" + l_c.Value);

        binaryTree.InsertLeafFromParent(99, l_c);

        binaryTree.Print();

        var l_c_p = binaryTree.Padre_FindParent(l_c);
        var bin_t2 = binaryTree.RemoveSubtree(l_c);


        Console.WriteLine("\nAlbero aggiornato_____");
        binaryTree.Print();

        Console.WriteLine("\nAlbero estratto_____");
        bin_t2.Print();

        binaryTree.InsertSubtree(l_c_p, bin_t2);
        Console.WriteLine("\n____________________");
        binaryTree.Print();
    }
}
