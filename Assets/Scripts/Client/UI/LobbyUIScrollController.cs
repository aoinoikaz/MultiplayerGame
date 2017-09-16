using UnityEngine;
using UnityEngine.UI;

public class LobbyUIScrollController : MonoBehaviour 
{
    public int PanelCount;
	public int PanelOffset;
	public float SnapSpeed;

	public GameObject[] panelPrefabs;
	public ScrollRect scrollRect;

    private GameObject[] instantiatedPanels;
	private Vector2[] panelPositions;
    private RectTransform contentRect;
    private Vector2 contentVector;
    private Vector2 previousPos;

    private int selectedPanelId;
	private bool isScrolling;


    // Use this for initialization
    void Awake () 
	{
		contentRect = GetComponent<RectTransform> ();
		instantiatedPanels = new GameObject[PanelCount];
		panelPositions = new Vector2[PanelCount];

        selectedPanelId = 0;

        // Iterate through the amount of panels we have
        for (int i = 0; i < PanelCount; i++)
        {
            Debug.Log(panelPrefabs[i]);
            // This is the first panel so instantiate at normal position
            instantiatedPanels[i] = Instantiate(panelPrefabs[i], transform, false);

            // Continue to next iteration
            if (i == 0)
                continue;

            // Transform position of panels to their local position relative to UI
            instantiatedPanels[i].transform.localPosition = new Vector2(instantiatedPanels[i - 1].transform.localPosition.x + panelPrefabs[i].GetComponent<RectTransform>().sizeDelta.x + PanelOffset,
                instantiatedPanels[i].transform.localPosition.y);

            // cache the panel positions
            panelPositions[i] = -instantiatedPanels[i].transform.localPosition;
        }
    }


    void FixedUpdate()
	{
        // Disable intertia when we're on the first or last panel if we're not scrolling
        if (contentRect.anchoredPosition.x >= panelPositions [0].x && !isScrolling || contentRect.anchoredPosition.x <= panelPositions [panelPositions.Length - 1].x && !isScrolling) 
			scrollRect.inertia = false;

        float nearestPos = float.MaxValue;

        for (int i = 0; i < PanelCount; i++)
        {
            // If our current distance to the nearest position, meaning the
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - panelPositions[i].x);

            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanelId = i;
            }
        }

        // Set bounds on scroll velocity and remove intertia where applicable
		float scrollVelocity = Mathf.Abs (scrollRect.velocity.x);
		if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
		if (isScrolling || scrollVelocity > 400) return;
		if (isScrolling) return;

        // Smoothly scroll from panel to panel
		contentVector.x = Mathf.SmoothStep (contentRect.anchoredPosition.x, 
			panelPositions[selectedPanelId].x, SnapSpeed * Time.fixedDeltaTime);

		// Set the rects position to the interpolated new pos based on snap speed
		contentRect.anchoredPosition = contentVector;
	}


    // This is invoked when BeginDrag and EndDrag events are triggered
	public void Scrolling(bool scroll)
	{
		isScrolling = scroll;

		if (scroll)
			scrollRect.inertia = true;
	}


    public GameObject[] Panels
    {
        get { return instantiatedPanels; }
    }
}