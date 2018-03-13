using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

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
            ListerJeuSelectionné();

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


    //Fonction qui liste le jeu sélectionné dans la page précédente.
    public void ListerJeuSelectionné()
    {
        //<sspeichert>
        //On récupère en get la clé du site web.
        string codeJeu = Request.QueryString["codeJeu"];
        //</sspeichert>
        //Gestion des exceptions essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];


                //<sspeichert>



                OleDbDataReader readerSelect = modele.ReadClient("SELECT Jeu.CodeJeu, Jeu.Titre, Jeu.Prix, Jeu.Plateforme, TypeJeu.Genre, Jeu.CodeCompagnie, Jeu.Description, Jeu.Image FROM Jeu INNER JOIN TypeJeu ON Jeu.IdGenre = TypeJeu.IdGenre WHERE Jeu.CodeJeu ='" + codeJeu + "'");

                 //Pendant que le reader est en train d'être lu, on entre 
                while (readerSelect.Read())
                {
                    if (codeJeu == readerSelect[0].ToString())
                    {
                        //ImageJeu.ImageUrl = "~/img/miniatures/" + readerSelect["Image"].ToString();
                        LabelTitreDuJeu.Text = "Titre du jeu :" + readerSelect["Titre"].ToString();
                        LabelGenre.Text = "Genre :" + readerSelect["Genre"].ToString();
                        LabelPlateforme.Text = "Plateforme :" + readerSelect["Plateforme"].ToString();
                        LabelPrix.Text = "Prix :" + readerSelect["Prix"].ToString();
                        LabelDeveloppeur.Text = "Developpeur :" + readerSelect["CodeCompagnie"].ToString();
                        LabelSynopsis.Text = "Synopsis :" + readerSelect["Description"].ToString();
                    }

                }

                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                    //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    readerSelect.Close();
                //<sspeichert>
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