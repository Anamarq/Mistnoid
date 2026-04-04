using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private string pieceID;
    [SerializeField] private BookPage parentPage;
    public BookPage ParentPage => parentPage;
    public string PieceID => pieceID;

    private bool obtained;
    public bool Obtained => obtained;   

    public void SetObtained(bool state)
    {
        obtained = state;
        gameObject.SetActive(state);
    }
    
}