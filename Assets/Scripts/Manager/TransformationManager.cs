using System.Collections;
using Extension;
using UnityEngine;

namespace Manager
{
    public class TransformationManager : Singleton<TransformationManager>
    {
        private int _transformationTime;

        public void Transformation(TransformationStruct transformation)
        {
            _transformationTime = transformation.plantData.GetTransformationTime(transformation.transformationType);
            StartCoroutine(nameof(TransformationProcess), transformation);
        }

        private IEnumerator TransformationProcess(TransformationStruct transformation)
        {
            yield return new WaitForSeconds(_transformationTime);
            WindowManager.Instance.TransformationComplete(transformation);
        }
    }
}