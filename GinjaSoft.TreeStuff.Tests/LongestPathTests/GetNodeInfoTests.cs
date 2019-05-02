namespace GinjaSoft.TreeStuff.Tests.LongestPathTests
{
  using Xunit;
  using Xunit.Abstractions;


  public class GetNodeInfoTests
  {
    //
    // Private data
    //

    private readonly ITestOutputHelper _output;


    //
    // Constructor
    //

    public GetNodeInfoTests(ITestOutputHelper output)
    {
      _output = output;
    }


    //
    // Public methods
    //

    [Fact]
    public void SingleNode()
    {
      //          50

      var tree = new Node<uint>(50) { };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(1u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void OneLeftChildIncreasing()
    {
      //          50
      //         /
      //       30

      var tree = new Node<uint>(50) {
        Left = new Node<uint>(30) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(2u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void OneLeftChildDecreasing()
    {
      //          50
      //         /
      //       70

      var tree = new Node<uint>(50) {
        Left = new Node<uint>(70) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(1u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void OneRightChildIncreasing()
    {
      //          50
      //            \
      //             30

      var tree = new Node<uint>(50) {
        Right = new Node<uint>(30) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(2u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void OneRightChildDecreasing()
    {
      //          50
      //            \
      //             70

      var tree = new Node<uint>(50) {
        Right = new Node<uint>(70) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(1u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void TwoChildrenWithNoThroughPath()
    {
      //          50
      //         /  \
      //       30    10

      var tree = new Node<uint>(50) {
        Left = new Node<uint>(30) { },
        Right = new Node<uint>(10) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(2u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void TwoChildrenWithThroughPath()
    {
      //          50
      //         /  \
      //       30    70

      var tree = new Node<uint>(50) {
        Left = new Node<uint>(30) { },
        Right = new Node<uint>(70) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(2u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(3u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void TwoLevelTree()
    {
      //          50
      //         /  \
      //       30    70
      //      /  \
      //    20    10

      var tree = new Node<uint>(50) {
        Left = new Node<uint>(30) {
          Left = new Node<uint>(20) { },
          Right = new Node<uint>(10) { }
        },
        Right = new Node<uint>(70) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(50u, nodeInfo.RootValue);
      Assert.Equal(3u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(4u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void MaxIncreasingDoesNotIncludeRoot()
    {
      //          10
      //         /
      //       30
      //         \
      //          20

      var tree = new Node<uint>(10) {
        Left = new Node<uint>(30) {
          Right = new Node<uint>(20) { }
        },
        Right = new Node<uint>(40) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(10u, nodeInfo.RootValue);
      Assert.Equal(1u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(2u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void ThroughPathDoesNotIncludeRoot()
    {
      //          10
      //         /  \
      //       30    40
      //      /  \
      //    40    20
      //            \
      //             10

      var tree = new Node<uint>(10) {
        Left = new Node<uint>(30) {
          Left = new Node<uint>(40) { },
          Right = new Node<uint>(20) {
            Right = new Node<uint>(10) { }
          }
        },
        Right = new Node<uint>(40) { }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(10u, nodeInfo.RootValue);
      Assert.Equal(1u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(3u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(4u, nodeInfo.MaxPathLen);
    }

    [Fact]
    public void DeepTree()
    {
      //          70
      //         /   \
      //       50     30
      //      /  \   /  \
      //    60   70 10   15
      //              \
      //               35
      //              /  \
      //             45   10
      //            /  \    \
      //          50    90   65
      //            \
      //             60

      var tree = new Node<uint>(70) {
        Left = new Node<uint>(50) {
          Left = new Node<uint>(60) { },
          Right = new Node<uint>(70) { }
        },
        Right = new Node<uint>(30) {
          Left = new Node<uint>(10) {
            Right = new Node<uint>(35) {
              Left = new Node<uint>(45) {
                Left = new Node<uint>(50) {
                  Right = new Node<uint>(60) { }
                },
                Right = new Node<uint>(90) { }
              },
              Right = new Node<uint>(10) {
                Right = new Node<uint>(65) { }
              }
            }
          },
          Right = new Node<uint>(15) { }
        }
      };

      var nodeInfo = GetNodeInfo(tree);

      Assert.Equal(70u, nodeInfo.RootValue);
      Assert.Equal(3u, nodeInfo.MaxIncPathLenToRoot);
      Assert.Equal(1u, nodeInfo.MaxDecPathLenToRoot);
      Assert.Equal(5u, nodeInfo.MaxPathLen);
    }


    //
    // Private methods
    //

    private LongestPath.NodeInfo GetNodeInfo(Node<uint> tree)
    {
      var nodeInfo = LongestPath.GetNodeInfo(tree);

      _output.WriteLine($"MaxIncreasingPathLengthToRoot={nodeInfo.MaxIncPathLenToRoot}");
      _output.WriteLine($"MaxDecreasingPathLengthToRoot={nodeInfo.MaxDecPathLenToRoot}");
      _output.WriteLine($"MaxPathLength={nodeInfo.MaxPathLen}");

      return nodeInfo;
    }
  }
}
