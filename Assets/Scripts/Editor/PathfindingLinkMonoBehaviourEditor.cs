using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingLinkMonoBehaviour))]
public class PathfindingLinkMonoBehaviourEditor : Editor
{
    private void OnSceneGUI()
    {
        PathfindingLinkMonoBehaviour pathfindingLinkMonoBehaviour = (PathfindingLinkMonoBehaviour)target;

        // Active la vérification des changements, pour que la position de notre objet 
        // contenant le script PathfindingLinkMonoBehaviour
        // puisse être mise à jour si on manupile le handle créé à la ligne suivante.
        EditorGUI.BeginChangeCheck();

        // Crée un Handle (les trois flèches directionnelles de la Scene View) à la position spéficiée,
        //avec l'orientation spécifiée.
        Vector3 newLinkPositionA = Handles.PositionHandle(
            pathfindingLinkMonoBehaviour.linkPositionA, Quaternion.identity);
        Vector3 newLinkPositionB = Handles.PositionHandle(
            pathfindingLinkMonoBehaviour.linkPositionB, Quaternion.identity);

        // C'est ici, s'il y a eu un changement, qu'on met à jour la position de l'objet contenant 
        // le script PathfindingLinkMonoBehaviour.
        if (EditorGUI.EndChangeCheck())
        {
            // Pour permettre de revenir en arrière.
            Undo.RecordObject(pathfindingLinkMonoBehaviour, "Change Link Position");

            pathfindingLinkMonoBehaviour.linkPositionA = newLinkPositionA;
            pathfindingLinkMonoBehaviour.linkPositionB = newLinkPositionB;
        }
    }
}





