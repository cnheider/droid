﻿namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class Unobservables {
    public Unobservables(ref System.Collections.Generic.IList<UnityEngine.Rigidbody> rigidbodies,
                         ref System.Collections.Generic.IList<UnityEngine.Transform> transforms) {
      if (rigidbodies != null) {
        this.Bodies = new Body[rigidbodies.Count];
        for (var i = 0; i < this.Bodies.Length; i++) {
          /*if (rigidbodies[i] != null) { //TODO: Proper way to construct unobservables in case somethings with destroyed but if this is the case then reinitialisation of a state wont work anyway and lets just crash.
            this.Bodies[i] = new Body(rigidbodies[i].velocity, rigidbodies[i].angularVelocity);
          }*/
          this.Bodies[i] = new Body(vel : rigidbodies[index : i].velocity,
                                    ang : rigidbodies[index : i].angularVelocity);
        }
      }

      if (transforms != null) {
        this.Poses = new UnityEngine.Pose[transforms.Count];
        for (var i = 0; i < this.Poses.Length; i++) {
          /*if (transforms[i] != null) { //TODO: Proper way to construct unobservables in case somethings with destroyed but if this is the case then reinitialisation of a state wont work anyway and lets just crash.
            this.Poses[i] = new Pose(transforms[i].position, transforms[i].rotation);
          }*/
          this.Poses[i] = new UnityEngine.Pose(position : transforms[index : i].position,
                                               rotation : transforms[index : i].rotation);
        }
      }
    }

    public Unobservables(ref Body[] bodies, ref UnityEngine.Pose[] poses) {
      this.Bodies = bodies;
      this.Poses = poses;
    }

    public Unobservables() { }

    public Unobservables(ref UnityEngine.Rigidbody[] rigidbodies, ref UnityEngine.Transform[] transforms) {
      if (rigidbodies != null) {
        this.Bodies = new Body[rigidbodies.Length];
        for (var i = 0; i < this.Bodies.Length; i++) {
          this.Bodies[i] = new Body(vel : rigidbodies[i].velocity, ang : rigidbodies[i].angularVelocity);
        }
      }

      if (transforms != null) {
        this.Poses = new UnityEngine.Pose[transforms.Length];
        for (var i = 0; i < this.Poses.Length; i++) {
          /*if (transforms[i] != null) { //TODO: Proper way to construct unobservables in case somethings with destroyed but if this is the case then reinitialisation of a state wont work anyway and lets just crash.
            this.Poses[i] = new Pose(transforms[i].position, transforms[i].rotation);
          }*/
          this.Poses[i] =
              new UnityEngine.Pose(position : transforms[i].position, rotation : transforms[i].rotation);
        }
      }
    }

    /// <summary>
    /// </summary>
    public Body[] Bodies { get; } = { };

    /// <summary>
    /// </summary>
    public UnityEngine.Pose[] Poses { get; } = { };

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var poses_str = "";
      if (this.Poses != null) {
        foreach (var pose in this.Poses) {
          poses_str += pose + "\n";
        }
      }

      var bodies_str = "";
      if (this.Bodies != null) {
        foreach (var body in this.Bodies) {
          bodies_str += body + "\n";
        }
      }

      return $"<Unobservables>\n {poses_str},{bodies_str}\n</Unobservables>\n";
    }
  }
}