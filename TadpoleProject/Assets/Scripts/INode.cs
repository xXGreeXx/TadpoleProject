using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public interface INode
    {
        //propagate signal to output axons
        void PropagateSignalToAxons(int offset);

        //get neighbors
        List<GameObject> getNeighbors();

        //set neighbors
        void setNeighbors(List<GameObject> neighbors);

        //add output axon
        void AddAxon(GameObject axon);

        //get axons
        List<GameObject> GetAxons();
    }
}
