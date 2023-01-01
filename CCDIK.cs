
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public enum axisType
{
    non,
    rootXY,
    rootYZ,
    rootXZ
}

public class CCDIK : UdonSharpBehaviour
{
    [SerializeField] int loopNum = 5;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform[] bones = default;
    [SerializeField] axisType axistype = axisType.non;

    private Vector3 constraintAxis;

    private void Start()
    {
        if (axistype != axisType.non)
        {
            if(axistype == axisType.rootXY)
            {
                constraintAxis = bones[0].forward;
            }
            else if (axistype == axisType.rootYZ)
            {
                constraintAxis = bones[0].right;
            }
            else if (axistype == axisType.rootXZ)
            {
                constraintAxis = bones[0].up;
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < loopNum; i++)
        {
            for(int j = bones.Length - 2; j >= 0; j--)
            {
                Vector3 boneVec;
                Vector3 targetVec;

                if (axistype != axisType.non)
                {
                    boneVec = Vector3.ProjectOnPlane(bones[bones.Length - 1].position - bones[j].position, constraintAxis).normalized;
                    targetVec = Vector3.ProjectOnPlane(targetPosition.position - bones[j].position, constraintAxis).normalized;
                }
                else
                {
                    boneVec = (bones[bones.Length - 1].position - bones[j].position).normalized;
                    targetVec = (targetPosition.position - bones[j].position).normalized;
                    
                }

                float rotateAngle = Mathf.Acos(Vector3.Dot(boneVec, targetVec));
                if (rotateAngle > 1.0e-5f)
                {
                    Vector3 rotateAxis = Vector3.Cross(boneVec, targetVec).normalized;
                    bones[j].rotation = Quaternion.AngleAxis(rotateAngle, rotateAxis) * bones[j].rotation;
                }
            }
        }
    }
}
