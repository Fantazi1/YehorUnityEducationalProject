using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform player;

    [Header("Camera positions")]
    public Transform[] cameraPos;

    public float cameraSpeed = 5f;

    private int currentRoomIndex = 0;
    private float cameraInitialZ;

    private Camera cam;
    private float roomWidth;
    private float roomHeight;

    void Start()
    {
        cam = GetComponent<Camera>();
        cameraInitialZ = transform.position.z;

        UpdateRoomDimensions();

        currentRoomIndex = FindRoomIndexForPlayer();

        transform.position = new Vector3(cameraPos[currentRoomIndex].position.x, cameraPos[currentRoomIndex].position.y, cameraInitialZ);
    }

    void LateUpdate()
    {
        if (player == null || cameraPos.Length == 0 || cam == null) return;

        UpdateRoomDimensions();

        int newRoomIndex = FindRoomIndexForPlayer();

        if (newRoomIndex != -1 && newRoomIndex != currentRoomIndex)
        {
            currentRoomIndex = newRoomIndex;
            //Debug.Log($"[Камера] Гравець перейшов до кімнати: {cameraPos[currentRoomIndex].name}");
        }

        Vector3 targetPosition = new Vector3(cameraPos[currentRoomIndex].position.x, cameraPos[currentRoomIndex].position.y, cameraInitialZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }

    void UpdateRoomDimensions()
    {
        roomHeight = cam.orthographicSize * 2f;
        roomWidth = roomHeight * cam.aspect;
    }

    int FindRoomIndexForPlayer()
    {
        Vector2 p = player.position;

        for (int i = 0; i < cameraPos.Length; i++)
        {
            if (cameraPos[i] == null) continue;

            Vector2 roomCenter = cameraPos[i].position;

            float minX = roomCenter.x - (roomWidth / 2f);
            float maxX = roomCenter.x + (roomWidth / 2f);
            float minY = roomCenter.y - (roomHeight / 2f);
            float maxY = roomCenter.y + (roomHeight / 2f);

            if (p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY)
            {
                return i;
            }
        }

        return currentRoomIndex;
    }

    void OnDrawGizmos()
    {
        if (cameraPos == null) return;

        Camera previewCam = GetComponent<Camera>();
        if (previewCam == null) return;

        float previewHeight = previewCam.orthographicSize * 2f;
        float previewWidth = previewHeight * previewCam.aspect;

        Gizmos.color = new Color(1f, 0.6f, 0f, 0.3f); 
        for (int i = 0; i < cameraPos.Length; i++)
        {
            if (cameraPos[i] != null)
            {
                Gizmos.DrawWireCube(cameraPos[i].position, new Vector3(previewWidth, previewHeight, 0.1f));
            }
        }
    }
}