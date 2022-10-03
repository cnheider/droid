namespace droid.Runtime.Sampling {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class RandomWalk : UnityEngine.MonoBehaviour //Self-Avoiding Random Walk algorithm
  {
    //How many steps do we want to take before we stop?
    public int stepsToTake;

    const float _eps = 0.001f;

    //The walk directions we can take
    System.Collections.Generic.List<UnityEngine.Vector3> _all_possible_directions =
        new System.Collections.Generic.List<UnityEngine.Vector3> {
                                                                     new UnityEngine.Vector3(x : 0f,
                                                                       y : 0f,
                                                                       z : 1f),
                                                                     new UnityEngine.Vector3(x : 0f,
                                                                       y : 0f,
                                                                       z : -1f),
                                                                     new UnityEngine.Vector3(x : -1f,
                                                                       y : 0f,
                                                                       z : 0f),
                                                                     new UnityEngine.Vector3(x : 1f,
                                                                       y : 0f,
                                                                       z : 0f)
                                                                 };

    //Final random walk positions
    System.Collections.Generic.List<UnityEngine.Vector3> _random_walk_positions;

    void Update() {
      if (UnityEngine.Input.GetKeyDown(key : UnityEngine.KeyCode.Return)) {
        this._random_walk_positions = this.GenerateSelfAvoidingRandomWalk();

        //Debug.Log(randomWalkPositions.Count);
      }

      //Display the path with lines
      if (this._random_walk_positions != null && this._random_walk_positions.Count > 1) {
        for (var i = 1; i < this._random_walk_positions.Count; i++) {
          UnityEngine.Debug.DrawLine(start : this._random_walk_positions[index : i - 1],
                                     end : this._random_walk_positions[index : i]);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public System.Collections.Generic.List<UnityEngine.Vector3> GenerateSelfAvoidingRandomWalk() {
      //Create the node we are starting from
      var start_pos = UnityEngine.Vector3.zero;

      var current_node = new WalkNode(pos : start_pos,
                                      previous_node : null,
                                      possible_directions :
                                      new System.Collections.Generic.List<UnityEngine.Vector3>(collection :
                                        this._all_possible_directions));

      //How many steps have we taken, so we know when to stop the algorithm
      var steps_so_far = 0;

      //So we do not visit the same node more than once
      var visited_nodes = new System.Collections.Generic.List<UnityEngine.Vector3> {start_pos};

      while (true) {
        //Check if we have walked enough steps
        if (steps_so_far == this.stepsToTake) {
          //Debug.Log("Found path");

          break;
        }

        //Need to backtrack if we cant move in any direction from the current node
        while (current_node._PossibleDirections.Count == 0) {
          current_node = current_node._PreviousNode;

          //Do not need to remove nodes that is not a part of the final path from the list of visited nodes
          //because there's no point in visiting them again because we know we cant find a path from those nodes

          steps_so_far -= 1;
        }

        //Walk in a random direction from this node
        var random_dir_pos =
            UnityEngine.Random.Range(minInclusive : 0, maxExclusive : current_node._PossibleDirections.Count);

        var random_dir = current_node._PossibleDirections[index : random_dir_pos];

        //Remove this direction from the list of possible directions we can take from this node
        current_node._PossibleDirections.RemoveAt(index : random_dir_pos);

        //Whats the position after we take a step in this direction
        var next_node_pos = current_node._Pos + random_dir;

        //Have we visited this position before?
        if (!HasVisitedNode(pos : next_node_pos, list_pos : visited_nodes)) {
          //Walk to this node
          current_node = new WalkNode(pos : next_node_pos,
                                      previous_node : current_node,
                                      possible_directions :
                                      new System.Collections.Generic.List<UnityEngine.Vector3>(collection :
                                        this._all_possible_directions));

          visited_nodes.Add(item : next_node_pos);

          steps_so_far += 1;
        }
      }

      //Generate the final path
      var random_walk_positions = new System.Collections.Generic.List<UnityEngine.Vector3>();

      while (current_node._PreviousNode != null) {
        random_walk_positions.Add(item : current_node._Pos);

        current_node = current_node._PreviousNode;
      }

      random_walk_positions.Add(item : current_node._Pos);

      //Reverse the list so it begins at the step we started from
      random_walk_positions.Reverse();

      return random_walk_positions;
    }

    //Checks if a position is in a list of positions
    static bool HasVisitedNode(UnityEngine.Vector3 pos,
                               System.Collections.Generic.List<UnityEngine.Vector3> list_pos) {
      var has_visited = false;

      foreach (var t in list_pos) {
        var dist_sqr = UnityEngine.Vector3.SqrMagnitude(vector : pos - t);

        if (dist_sqr < _eps) { //Cannot compare exactly because of floating point precisions
          has_visited = true;

          break;
        }
      }

      return has_visited;
    }

    /// <summary>
    ///  Help class to keep track of the steps
    /// </summary>
    class WalkNode {
      public UnityEngine.Vector3 _Pos; //The position of this node in the world

      public System.Collections.Generic.List<UnityEngine.Vector3>
          _PossibleDirections; //Which steps can we take from this node?

      public WalkNode _PreviousNode;

      public WalkNode(UnityEngine.Vector3 pos,
                      WalkNode previous_node,
                      System.Collections.Generic.List<UnityEngine.Vector3> possible_directions) {
        this._Pos = pos;

        this._PreviousNode = previous_node;

        this._PossibleDirections = possible_directions;
      }
    }
  }
}