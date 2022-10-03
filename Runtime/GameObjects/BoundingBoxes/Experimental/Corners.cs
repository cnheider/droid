﻿namespace droid.Runtime.GameObjects.BoundingBoxes.Experimental {
  /// <summary>
  /// </summary>
  public static class Corners {
    public static UnityEngine.Vector3[] ExtractCorners(UnityEngine.Vector3 v3_center,
                                                       UnityEngine.Vector3 v3_extents,
                                                       UnityEngine.Transform reference_transform = null) {
      var v3_front_top_left = new UnityEngine.Vector3(x : v3_center.x - v3_extents.x,
                                                      y : v3_center.y + v3_extents.y,
                                                      z : v3_center.z
                                                          - v3_extents.z); // Front top left corner
      var v3_front_top_right = new UnityEngine.Vector3(x : v3_center.x + v3_extents.x,
                                                       y : v3_center.y + v3_extents.y,
                                                       z : v3_center.z
                                                           - v3_extents.z); // Front top right corner
      var v3_front_bottom_left = new UnityEngine.Vector3(x : v3_center.x - v3_extents.x,
                                                         y : v3_center.y - v3_extents.y,
                                                         z : v3_center.z
                                                             - v3_extents.z); // Front bottom left corner
      var v3_front_bottom_right = new UnityEngine.Vector3(x : v3_center.x + v3_extents.x,
                                                          y : v3_center.y - v3_extents.y,
                                                          z : v3_center.z
                                                              - v3_extents.z); // Front bottom right corner
      var v3_back_top_left = new UnityEngine.Vector3(x : v3_center.x - v3_extents.x,
                                                     y : v3_center.y + v3_extents.y,
                                                     z : v3_center.z + v3_extents.z); // Back top left corner
      var v3_back_top_right = new UnityEngine.Vector3(x : v3_center.x + v3_extents.x,
                                                      y : v3_center.y + v3_extents.y,
                                                      z : v3_center.z
                                                          + v3_extents.z); // Back top right corner
      var v3_back_bottom_left = new UnityEngine.Vector3(x : v3_center.x - v3_extents.x,
                                                        y : v3_center.y - v3_extents.y,
                                                        z : v3_center.z
                                                            + v3_extents.z); // Back bottom left corner
      var v3_back_bottom_right = new UnityEngine.Vector3(x : v3_center.x + v3_extents.x,
                                                         y : v3_center.y - v3_extents.y,
                                                         z : v3_center.z
                                                             + v3_extents.z); // Back bottom right corner
      if (reference_transform) {
        v3_front_top_left = reference_transform.TransformPoint(position : v3_front_top_left);
        v3_front_top_right = reference_transform.TransformPoint(position : v3_front_top_right);
        v3_front_bottom_left = reference_transform.TransformPoint(position : v3_front_bottom_left);
        v3_front_bottom_right = reference_transform.TransformPoint(position : v3_front_bottom_right);
        v3_back_top_left = reference_transform.TransformPoint(position : v3_back_top_left);
        v3_back_top_right = reference_transform.TransformPoint(position : v3_back_top_right);
        v3_back_bottom_left = reference_transform.TransformPoint(position : v3_back_bottom_left);
        v3_back_bottom_right = reference_transform.TransformPoint(position : v3_back_bottom_right);
      }

      return new[] {
                       v3_front_top_left,
                       v3_front_top_right,
                       v3_front_bottom_left,
                       v3_front_bottom_right,
                       v3_back_top_left,
                       v3_back_top_right,
                       v3_back_bottom_left,
                       v3_back_bottom_right
                   };
    }

    /// <summary>
    /// </summary>
    /// <param name="v3_front_top_left"></param>
    /// <param name="v3_front_top_right"></param>
    /// <param name="v3_front_bottom_left"></param>
    /// <param name="v3_front_bottom_right"></param>
    /// <param name="v3_back_top_left"></param>
    /// <param name="v3_back_top_right"></param>
    /// <param name="v3_back_bottom_left"></param>
    /// <param name="v3_back_bottom_right"></param>
    /// <param name="color"></param>
    public static void DrawBox(UnityEngine.Vector3 v3_front_top_left,
                               UnityEngine.Vector3 v3_front_top_right,
                               UnityEngine.Vector3 v3_front_bottom_left,
                               UnityEngine.Vector3 v3_front_bottom_right,
                               UnityEngine.Vector3 v3_back_top_left,
                               UnityEngine.Vector3 v3_back_top_right,
                               UnityEngine.Vector3 v3_back_bottom_left,
                               UnityEngine.Vector3 v3_back_bottom_right,
                               UnityEngine.Color color) {
      UnityEngine.Debug.DrawLine(start : v3_front_top_left, end : v3_front_top_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_top_right, end : v3_front_bottom_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_bottom_right, end : v3_front_bottom_left, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_bottom_left, end : v3_front_top_left, color : color);

      UnityEngine.Debug.DrawLine(start : v3_back_top_left, end : v3_back_top_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_back_top_right, end : v3_back_bottom_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_back_bottom_right, end : v3_back_bottom_left, color : color);
      UnityEngine.Debug.DrawLine(start : v3_back_bottom_left, end : v3_back_top_left, color : color);

      UnityEngine.Debug.DrawLine(start : v3_front_top_left, end : v3_back_top_left, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_top_right, end : v3_back_top_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_bottom_right, end : v3_back_bottom_right, color : color);
      UnityEngine.Debug.DrawLine(start : v3_front_bottom_left, end : v3_back_bottom_left, color : color);
    }

    /// <summary>
    ///   Draws a Circle using Debug.Draw functions
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Radius of the circle.</param>
    /// <param name="color">Color for Debug.Draw.</param>
    /// <param name="num_segments">Number of segments for the circle, used for precision of the draw.</param>
    /// <param name="duration">Duration to show the circle.</param>
    public static void DrawCircle(UnityEngine.Vector2 center,
                                  float radius,
                                  UnityEngine.Color color,
                                  float num_segments = 40,
                                  float duration = 0.01f) {
      var rot_quaternion =
          UnityEngine.Quaternion.AngleAxis(angle : 360.0f / num_segments, axis : UnityEngine.Vector3.forward);
      var vertex_start = new UnityEngine.Vector2(x : radius, 0.0f);
      for (var i = 0; i < num_segments; i++) {
        UnityEngine.Vector2 rotated_point = rot_quaternion * vertex_start;

        // Draw the segment, shifted by the center
        UnityEngine.Debug.DrawLine(start : center + vertex_start,
                                   end : center + rotated_point,
                                   color : color,
                                   duration : duration);

        vertex_start = rotated_point;
      }
    }

    /// <summary>
    ///   Draws a box using Debug.Draw functions
    /// </summary>
    /// <param name="world_top_left">World top left corner.</param>
    /// <param name="world_bottom_right">World bottom right corner.</param>
    /// <param name="color">Color for Debug.Draw.</param>
    /// <param name="duration">Duration to show the box.</param>
    public static void DrawBox(UnityEngine.Vector2 world_top_left,
                               UnityEngine.Vector2 world_bottom_right,
                               UnityEngine.Color color,
                               float duration = 0.01f) {
      var world_top_right = new UnityEngine.Vector2(x : world_bottom_right.x, y : world_top_left.y);
      var world_bottom_left = new UnityEngine.Vector2(x : world_top_left.x, y : world_bottom_right.y);

      UnityEngine.Debug.DrawLine(start : world_top_left,
                                 end : world_bottom_left,
                                 color : color,
                                 duration : duration);
      UnityEngine.Debug.DrawLine(start : world_bottom_left,
                                 end : world_bottom_right,
                                 color : color,
                                 duration : duration);
      UnityEngine.Debug.DrawLine(start : world_bottom_right,
                                 end : world_top_right,
                                 color : color,
                                 duration : duration);
      UnityEngine.Debug.DrawLine(start : world_top_right,
                                 end : world_top_left,
                                 color : color,
                                 duration : duration);
    }

    /// <summary>
    ///   Draws an array of edges, where an edge is defined by two Vector2 points, using Debug.Draw
    /// </summary>
    /// <param name="world_points">World points, defining each vertex of an edge in world space.</param>
    /// <param name="color">Color for Debug.Draw.</param>
    /// <param name="duration">Duration to show the edges.</param>
    public static void DrawEdges(UnityEngine.Vector2[] world_points,
                                 UnityEngine.Color color,
                                 float duration = 0.01f) {
      // Draw each segment except the last
      for (var i = 0; i < world_points.Length - 1; i++) {
        UnityEngine.Vector3 next_point = world_points[i + 1];
        UnityEngine.Vector3 current_point = world_points[i];
        UnityEngine.Debug.DrawLine(start : current_point,
                                   end : next_point,
                                   color : color,
                                   duration : duration);
      }
    }

    /// <summary>
    ///   Draws a polygon, defined by all verteces of the polygon, using Debug.Draw
    /// </summary>
    /// <param name="world_points">World points, defining each vertex of the polygon in world space.</param>
    /// <param name="color">Color for Debug.Draw.</param>
    /// <param name="duration">Duration to show the polygon.</param>
    public static void DrawPolygon(UnityEngine.Vector2[] world_points,
                                   UnityEngine.Color color,
                                   float duration = 0.01f) {
      DrawEdges(world_points : world_points, color : color, duration : duration);

      // Polygons are just edges with the first and last points connected
      if (world_points.Length > 1) {
        UnityEngine.Debug.DrawLine(start : world_points[world_points.Length - 1],
                                   end : world_points[0],
                                   color : color,
                                   duration : duration);
      }
    }

    /// <summary>
    ///   Draws an arrow using Debug.Draw
    /// </summary>
    /// <param name="origin">Origin point in world space.</param>
    /// <param name="endpoint">Endpoint in world space.</param>
    /// <param name="color">Color for Debug.Draw.</param>
    /// <param name="duration">Duration to show the arrow.</param>
    public static void DrawArrow(UnityEngine.Vector2 origin,
                                 UnityEngine.Vector2 endpoint,
                                 UnityEngine.Color color,
                                 float duration = 0.01f) {
      // Draw the line that makes up the body of the arrow
      UnityEngine.Debug.DrawLine(start : origin,
                                 end : endpoint,
                                 color : color,
                                 0.01f);

      // Draw arrowhead so we can see direction
      var arrow_direction = endpoint - origin;
      DebugDrawArrowhead(origin : endpoint,
                         direction : arrow_direction.normalized,
                         size : GetArrowSizeForLine(line : arrow_direction),
                         color : color,
                         duration : duration);
    }

    static float GetArrowSizeForLine(UnityEngine.Vector2 line) {
      const float default_arrow_percentage = 0.05f;
      return (line * default_arrow_percentage).magnitude;
    }

    static void DebugDrawArrowhead(UnityEngine.Vector2 origin,
                                   UnityEngine.Vector2 direction,
                                   float size,
                                   UnityEngine.Color color,
                                   float duration = 0.01f,
                                   float theta = 30.0f) {
      // Theta angle is the acute angle of the arrow, so flip direction or else arrow will be pointing "backwards"
      var arrowhead_handle = -direction * size;

      var arrow_rotation_r =
          UnityEngine.Quaternion.AngleAxis(angle : theta, axis : UnityEngine.Vector3.forward);
      UnityEngine.Vector2 arrowhead_r = arrow_rotation_r * arrowhead_handle;
      UnityEngine.Debug.DrawLine(start : origin,
                                 end : origin + arrowhead_r,
                                 color : color,
                                 duration : duration);

      var arrow_rotation_l =
          UnityEngine.Quaternion.AngleAxis(angle : -theta, axis : UnityEngine.Vector3.forward);
      UnityEngine.Vector2 arrowhead_l = arrow_rotation_l * arrowhead_handle;
      UnityEngine.Debug.DrawLine(start : origin,
                                 end : origin + arrowhead_l,
                                 color : color,
                                 duration : duration);
    }
  }
}