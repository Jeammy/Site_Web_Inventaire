using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Net;
using System.Drawing;

public partial class _Default : System.Web.UI.Page
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


    protected void ButtonEnvoyer_Click(object sender, EventArgs e)
    {
        //Gestion des exception essentielle, on gère du code "dangereux"
        try
        {

            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //<sspeichert>  

                    //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                    Modele modele = (Modele)Session["modeleClient"];
                    //Si les boîtes de textes ont plus que zéro caractères
                    if (TextBoxCommentaire.Text.Length > 0 && TextBoxPrenom.Text.Length > 0 && TextBoxNom.Text.Length > 0)
                    {
                        //On exécute la requète
                        int rows = modele.CreateClient("INSERT INTO COMMENTAIRE (DateCreation, CommentaireEcrit, iDTypeClient, Prenom, Nom) VALUES (Now(),'" + TextBoxCommentaire.Text + "',4,'" + TextBoxPrenom.Text + "','" + TextBoxNom.Text + "')");
                        //Et on change les contrôles s'il y a au moins une ligne d'insérée dans la base de données!
                        if (rows > 0)
                        {
                            LabelValidation.ForeColor = Color.Green;
                            LabelValidation.Text = "Votre commentaire a été envoyé!";
                            LabelCommentaire.Visible = false;
                            LabelNom.Visible = false;
                            LabelPrenom.Visible = false;
                            TextBoxCommentaire.Visible = false;
                            TextBoxNom.Visible = false;
                            TextBoxPrenom.Visible = false;
                            ButtonEnvoyer.Visible = false;
                        }
                    }
                    else     //Sinon, un message d'erreur s'affiche, et c'est la que les validateurs s'activent.
                    {
                        LabelValidation.ForeColor = Color.DarkRed;
                        LabelValidation.Text = "D'Oh! Il y a eu un problème concernant votre connection...";
                    }
                //</sspeichert>
            }
        }

        catch (Exception exc)
        {
            //Si le probleme provenait de la transaction du modele, on la Rollback
            Modele modele = (Modele)Session["modeleClient"];
            modele.RollbackTransaction();
            //et on invalide le modele, il pourra être reconstruit en PostBack
            Session["modeleClient"] = null;
            System.Diagnostics.Debug.Write(exc);
        }

    }
}