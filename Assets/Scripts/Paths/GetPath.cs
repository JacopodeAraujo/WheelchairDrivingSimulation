using UnityEngine;

public class GetPath : MonoBehaviour
{
    public GameObject[] allPaths;
    VehicleMoveOnPath vehPath;
    // Start is called before the first frame update
    void Start()
    {
        //int num = Random.Range(0, allPaths.Length);
        //transform.position = allPaths[num].transform.position;
        //vehPath = GetComponent<VehicleMoveOnPath>();
        //vehPath.pathName = allPaths[num].name;
    }
    public string GetVehiclePath()
    {
        int num = Random.Range(0, allPaths.Length);

        return allPaths[num].name;
    }
}
