using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlantingController
{
    private PlantingInterface adapter;
    public PlantingInterface Adapter => adapter;
    
    public PlantingController(PlantingInterface adapter) {
        this.adapter = adapter;
    }


}

public interface PlantingInterface {
    public GameObject Instantiate(String name, Vector3 pos);
    public void Destroy(String name, Vector3 pos);
}