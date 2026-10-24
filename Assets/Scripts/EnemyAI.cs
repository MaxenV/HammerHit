
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 4f;

    [Header("Board")]
    [SerializeField] private SpriteRenderer boardRenderer;
    [SerializeField] private int gridWidth = 6;
    [SerializeField] private int gridHeight = 6;
    [SerializeField] private LayerMask obstacles;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Vector2Int lastGoal;
    private bool hasTarget;
    private float nextPathCheck;

    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (boardRenderer == null)
            return;

        Bounds bounds = boardRenderer.bounds;

        Debug.Log($"Board size: {bounds.size}");
        Debug.Log($"Cell size: {GetCellSize()}");

        lastGoal = new Vector2Int(-1, -1);
    }

    private Vector2 GetCellSize()
    {
        Bounds bounds = boardRenderer.bounds;

        return new Vector2(
            bounds.size.x / gridWidth,
            bounds.size.y / gridHeight
        );
    }

    private Vector2Int WorldToGrid(Vector2 position)
    {
        Bounds bounds = boardRenderer.bounds;
        Vector2 size = GetCellSize();

        return new Vector2Int(
            Mathf.Clamp(
                Mathf.FloorToInt(
                    (position.x - bounds.min.x) / size.x
                ),
                0, gridWidth - 1
            ),
            Mathf.Clamp(
                Mathf.FloorToInt(
                    (position.y - bounds.min.y) / size.y
                ),
                0, gridHeight - 1
            )
        );
    }

    private Vector2 GridToWorld(Vector2Int cell)
    {
        Bounds bounds = boardRenderer.bounds;
        Vector2 size = GetCellSize();

        return new Vector2(
            bounds.min.x + (cell.x + 0.5f) * size.x,
            bounds.min.y + (cell.y + 0.5f) * size.y
        );
    }

    private bool IsWalkable(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= gridWidth ||
            cell.y < 0 || cell.y >= gridHeight)
            return false;

        Vector2 center = GridToWorld(cell);
        Vector2 size = GetCellSize() * 0.7f;

        return Physics2D.OverlapBox(
            center,
            size,
            0f,
            obstacles
        ) == null;
    }

    private void FixedUpdate()
    {
        if (player == null || boardRenderer == null)
            return;

        Vector2Int goal = WorldToGrid(player.position);

        bool reachedTarget = hasTarget &&
            Vector2.Distance(rb.position, targetPosition) < 0.05f;

        if (Time.time >= nextPathCheck &&
            (goal != lastGoal || !hasTarget || reachedTarget))
        {
            FindPath();
            lastGoal = goal;
            nextPathCheck = Time.time + 0.2f;
        }

        if (!hasTarget)
            return;

        Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);
    }

    private void FindPath()
    {
        Vector2Int start = WorldToGrid(rb.position);
        Vector2Int goal = WorldToGrid(player.position);

        if (start == goal)
        {
            targetPosition = player.position;
            hasTarget = true;
            return;
        }

        Queue<Vector2Int> queue = new();
        HashSet<Vector2Int> visited = new();
        Dictionary<Vector2Int, Vector2Int> previous = new();

        queue.Enqueue(start);
        visited.Add(start);

        bool found = false;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goal)
            {
                found = true;
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;

                if (visited.Contains(next))
                    continue;

                if (!IsWalkable(next))
                    continue;

                visited.Add(next);
                previous[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!found)
        {
            hasTarget = false;
            return;
        }

        Vector2Int step = goal;

        while (previous[step] != start)
        {
            step = previous[step];
        }

        targetPosition = GridToWorld(step);
        hasTarget = true;
    }
}
