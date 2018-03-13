using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Mandat 2 par Jeammy Côté.
public partial class Commander : System.Web.UI.Page
{
    private ConnexionBD bd = null;
    private Modele modeleClient = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ListerClients();
        }
    }
    /// <summary>
    /// Initialisation, première étape du cycle de vie donc on en profite pour créer notre modèle seulement s'il n'existe pas dans la session
    /// Après quoi, on ne le referra pas inutilement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        //Construction des items dans la liste déroulente quantité.
        for (int i = 0; i < 30; i++)
        {
            ListItem item = new ListItem();
            //la valeur de l'item est égal à son texte.
            item.Text = (i + 1).ToString();
            item.Value = item.Text;

            DropDownListQuantite.Items.Add(item);
        }
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
                OleDbConnection connection = bd.ConnectToDB(SqlDataSource1.ConnectionString);

                //On Instancie notre modèle client en lui passant la connection reçue
                modeleClient = new Modele(connection);

                //Si tout a fonctionné, on stocke notre modèle dans la session. C'est la meilleure façon car un PostBack va tout effacer ce qu'on vient de faire !
                Session["modeleClient"] = modeleClient;
            }

            catch (Exception exc)
            {
                //message d'erreur comme quoi la BD ne sera pas disponible
                //modeleClient.RollbackTransaction();
                System.Diagnostics.Debug.Write(exc);
            }
        }
    }

    /// <summary>
    /// Cette méthode liste tous les jeux et les entrepots de la base de données et envoie les données en construction dans la table
    /// </summary>
    public void ListerClients()
    {
        //Gestion des exception essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];
                
                //tout les jeux et leur code.
                OleDbDataReader readerSelectJeu = modele.ReadClient("SELECT Titre, CodeJeu FROM Jeu");
                //On envoie notre table en construction
                ConstruireDropDownList(DropDownListJeu, readerSelectJeu, false);
                    
                //!!!!!!!!!!!!!!!!!!!!!!!!
                //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                //!!!!!!!!!!!!!!!!!!!!!!!!
                readerSelectJeu.Close();

                //Tout les ville et les pays des entrepot.
                OleDbDataReader readerSelectEntrepot = modele.ReadClient("SELECT Ville, Pays, IdEntrepot FROM Entrepot");
                //On envoie notre table en construction
                ConstruireDropDownList(DropDownListEntrepot, readerSelectEntrepot, true);

                //!!!!!!!!!!!!!!!!!!!!!!!!
                //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                //!!!!!!!!!!!!!!!!!!!!!!!!
                readerSelectEntrepot.Close();

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
    /// <summary>
    //Cette méthode effectue une insertion, mais "en memoire" dans la BD. Pour un impact réel dans le fichier de base de données
    //Il ne faut pas oublier d'appler le CommitChanges au déchargement de la page
    /// </summary>
    public void MettreAJourClient()
    {
        //Gestion des exception essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];

                //On peut maintenant faire notre requete
                int numRows = modele.CreateClient("INSERT INTO CommandeJeu (CodeJeu, IdEntrepot, Quantité, DateCommande) VALUES ('" +DropDownListJeu.SelectedValue+ "' ,"+int.Parse(DropDownListEntrepot.SelectedValue)+" ,'"+DropDownListQuantite.SelectedValue+"' ,NOW());");
                
                if (numRows >= 1)
                {
                    //message de succès à l'écran
                    PanelCommande.Enabled = false;
                    LabelConfirmation.Text = "Commande enregistrée.";
                }

                else
                {
                    //message d'erreur à l'écran
                    LabelConfirmation.Text = "Il y a eu une erreur avec votre commande veuillez réessayer ultérieurement ou contacter votre administrateur réseau.";
                }
                 
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

    /// <summary>
    /// Méthode qui construit la liste déroulante située dans le formulaire Commander.aspx
    /// </summary>
    /// <param name="dropDownList">l'objet table que l'on va construire dynamiquement</param>
    /// <param name="reader">le reader qui contient tous les enregistrements retenus par la requête de sélection</param>
    public void ConstruireDropDownList(DropDownList dropDownList, OleDbDataReader reader, bool IsEntrepot)
    {
        //extraction des enregistrements, on boucle le reader
        while (reader.Read())
        {
            ListItem item = null;

            item = new ListItem();
            
            item.Text = reader[0].ToString() + " - " + reader[1].ToString();
            item.Value = reader[1].ToString();

            //Enregistre l'id de l'entrepot dans la valeur de l'item.
            if (IsEntrepot)
            {
                item.Value = reader["IdEntrepot"].ToString();
            }
            dropDownList.Items.Add(item);
            
        }
    }
    /// <summary>
    /// Écouteur du click pour le BoutonCommander dans le formulaire Commander.aspx
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonCommander_Click(object sender, EventArgs e)
    {
        MettreAJourClient();
        //On met à jour le visuel immédiatement
        ListerClients();
    }
}