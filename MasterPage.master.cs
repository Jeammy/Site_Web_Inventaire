using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.ServiceModel.Channels;

//<SSPEICHERT­>
public partial class MasterPage : System.Web.UI.MasterPage
{
    //propriété qui représente notre classe de connexion et qui offre une passerelle vers la vraie connexion avec le pilote
    ConnexionBD bd = null;
    //propriété qui représente l'accès aux données (CRUD) pour un client
    Modele modeleClient = null;
    
    /// <summary>
    /// Initialisation, première étape du cycle de vie donc on en profite pour créer notre modèle seulement s'il n'existe pas dans la session
    /// Après quoi, on ne le referra pas inutilement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        //On ouvre pas une connexion si le modèle est déjà construit !
        //Il y a une nuance avec le postback ici car si on a frappé un problème dans le code, on aura mis le modèle à null dans la session
        //Ceci nous laisse donc une chance de le reconstruire !
        if (Session["modeleClient"] == null)
        {
            //Gestion des exception essentielle, on gère du code "dangereux"
            try
            {
                //On effectue la connexion et on l'obtient en retour
                bd = new ConnexionBD();
                OleDbConnection connection = bd.ConnectToDB(sqlDataSource1.ConnectionString);

                //On Instancie notre modèle client en lui passant la connection reçue
                modeleClient = new Modele(connection);

                //Si tout a fonctionné, on stocke notre modèle dans la session. C'est la meilleure façon car un PostBack va tout effacer ce qu'on vient de faire !
                Session["modeleClient"] = modeleClient;
            }
            
            catch (Exception exc)
            {
                //message d'erreur comme quoi la BD ne sera pas disponible
                modeleClient.RollbackTransaction();
                System.Diagnostics.Debug.Write(exc);
            }
        }
    }

    /// <summary>
    /// Chargement de la page, il faut faire attention au PostBack pour ne pas faire des requêtes inutilement à chaque PostBack
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Pas besoin de lister les clients à chaque postback !
        if (!IsPostBack)
        {
         
        }
        //<JCOTE>
        if (Session["adminConnected"] == null)
        {
            Session["adminConnected"] = false;

        }
        if ((bool)Session["adminConnected"] == true)
        {
            PanelAdmin.Visible = false;
            ImageButtonPasserUneCommande.Enabled = true;
            ImageButtonPasserUneCommande.Visible = true;
            ImageButtonDeconnection.Enabled = true;
            ImageButtonDeconnection.Visible = true;
        }
        else if ((bool)Session["adminConnected"] == false)
        {
            PanelAdmin.Visible = true;
            ImageButtonPasserUneCommande.Enabled = false;
            ImageButtonPasserUneCommande.Visible = false;
            ImageButtonDeconnection.Enabled = false;
            ImageButtonDeconnection.Visible = false;
        }
        //</JCOTE>

    }


    protected void Page_Unload(object sender, EventArgs e)
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!
        //CECI EST ESSENTIEL LORSQU'ON A FAIT DES REQUETES EN MODIFICATION CAR ELLES ÉTAIENT EN MEMOIRE DANS LA TRANSACTION, IL FAUT LES ÉCRIRE DANS LA BD
        //!!!!!!!!!!!!!!!!!!!!!!!!
        try
        {
            if (Session["modeleClient"] != null)
            {
                ((Modele)Session["modeleClient"]).CommitChanges();
            }
        }

        catch (Exception exc)
        {
            System.Diagnostics.Debug.Write(exc);
        }


    }

    //Si le bouton "Se connecter" est cliqué.
    protected void ButtonConnect_Click(object sender, EventArgs e)
    {
        string lastName = TextBoxLastName.Text;
        string firstName = TextBoxFirstName.Text;

        if (Session["modeleClient"] != null)
        {
            //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
            Modele modele = (Modele)Session["modeleClient"];

            OleDbDataReader readerSelect = modele.ReadClient("SELECT * FROM CLIENT WHERE iDTypeClient = 4");

            string iD = readerSelect.GetName(1);
            string iD2 = readerSelect.GetName(2);

           

            while (readerSelect.Read())
            {
                if (lastName == readerSelect[iD].ToString() && firstName == readerSelect[iD2].ToString())
                {
                   //<JCOTE>
                   PanelAdmin.Visible = false;
                   ImageButtonPasserUneCommande.Enabled = true;
                   ImageButtonPasserUneCommande.Visible = true;
                   ImageButtonDeconnection.Enabled = true;
                   ImageButtonDeconnection.Visible = true;
                   Session["adminConnected"] = true;
                   //</JCOTE>
                }
            }
            readerSelect.Close();
        }
    }
    //<JCOTE>
    protected void ImageButtonDeconnection_Click(object sender, ImageClickEventArgs e)
    {
        Session["adminConnected"] = false;
    }
    //</JCOTE>
}

//</SSPEICHERT­>