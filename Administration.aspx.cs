using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Mandat 2 par Jeammy Côté.
public partial class _Default : System.Web.UI.Page
{
    private ConnexionBD bd = null;
    private Modele modeleClient = null;

    //Détermine si la liste doit être en ordre ascendant ou descendant de date.
    private bool orderByAsc = true;

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
                modeleClient.RollbackTransaction();
                System.Diagnostics.Debug.Write(exc);
            }
        }
    }

    /// <summary>
    /// Cette méthode liste tous les clients de la base de données et envoie les données en construction dans la table
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

                //Récupère touts les jeux de la BD et les enregistre dans le reader avec un trie en ordre Ascendant ou descendant.
                if (orderByAsc)
                {
                    OleDbDataReader readerSelect = modele.ReadClient("SELECT Jeu.Titre, Jeu.Prix, CommandeJeu.CodeJeu, CommandeJeu.Quantité, CommandeJeu.DateCommande,CommandeJeu.IdCommande, Entrepot.Ville, Entrepot.Pays" +
                                                                 " FROM Jeu INNER JOIN (CommandeJeu INNER JOIN Entrepot ON CommandeJeu.IdEntrepot=Entrepot.IdEntrepot) ON Jeu.CodeJeu=CommandeJeu.CodeJeu " +
                                                                 "ORDER BY CommandeJeu.DateCommande");
                    //On envoie notre table en construction
                    ConstruireTable(Jeux, readerSelect);

                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                    //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    readerSelect.Close();
                }
                else 
                {
                    OleDbDataReader readerSelect = modele.ReadClient("SELECT Jeu.Titre, Jeu.Prix, CommandeJeu.CodeJeu, CommandeJeu.Quantité, CommandeJeu.DateCommande,CommandeJeu.IdCommande, Entrepot.Ville, Entrepot.Pays" +
                                                                 " FROM Jeu INNER JOIN (CommandeJeu INNER JOIN Entrepot ON CommandeJeu.IdEntrepot=Entrepot.IdEntrepot) ON Jeu.CodeJeu=CommandeJeu.CodeJeu " +
                                                                 "ORDER BY CommandeJeu.DateCommande DESC");
                    //On envoie notre table en construction
                    ConstruireTable(Jeux, readerSelect);

                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                    //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    readerSelect.Close();
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
    /// Méthode qui construit la table dynamique située dans le formulaire .aspx
    /// </summary>
    /// <param name="table">l'objet table que l'on va construire dynamiquement</param>
    /// <param name="reader">le reader qui contient tous les enregistrements retenus par la requête de sélection</param>
    public void ConstruireTable(Table table, OleDbDataReader reader)
    {
        TableRow headerRow = new TableRow();
        TableHeaderCell cell = null;

        //extraction des champs pour créer l'entete
        for (int champ = 0; champ < reader.FieldCount; champ++)
        {
            if (reader.GetName(champ) != "IdCommande")
            {
                cell = new TableHeaderCell();
                //On récupère le nom des champs ici
                cell.Text = reader.GetName(champ);
                //et ils deviennent le texte de nos entêtes
                headerRow.Cells.Add(cell);
            }
        }

        //ajout d'une colonne qui ne vient pas de la base de données !
        cell = new TableHeaderCell();
        cell.Text = "modifier";
        headerRow.Cells.Add(cell);

        //On ajoute la ligne des entêtes dans la table
        table.Rows.Add(headerRow);

        //extraction des enregistrements, on boucle le reader
        while (reader.Read())
        {
            TableRow row = new TableRow();
            TableCell tableCell = null;

            //Attention, on ne se rendra pas jusqu'au bout car la dernière entete ne provient pas de la BD !!!
            int numColumns = headerRow.Cells.Count - 1;

            //on se sert des noms de nos entetes comme clé pour récupérer nos enregistrements, encore une fois en ignorant la dernière qui a été créée manuellement !
            for (int i = 0; i < numColumns; i++)
            {
                if (reader[headerRow.Cells[i].Text] != reader["IdCommande"])
                {
                    tableCell = new TableCell();
                    //on se sert du nom de l'entête ici comme clé
                    tableCell.Text = reader[headerRow.Cells[i].Text].ToString();
                    row.Cells.Add(tableCell);
                }
                
            }

            //On popule la dernière colonne en créant un hyperlien qui enverra le id en GET
            HyperLink link = new HyperLink();
            //La valeur du id se trouve dans la première cellule de la rangée courante, d'où le [0] pour aller le chercher
            link.NavigateUrl = "ModifierCommande.aspx?IdCommande="+ reader["IdCommande"].ToString();
            link.Text = "Modifier la commande";

            //L'hyperlien étant un contrôle, on l'ajoute dans la cellule
            tableCell = new TableCell();
            tableCell.Controls.Add(link);
            row.Cells.Add(tableCell);

            //Et on ajoute notre ligne dans la table, on recommencera ce traitement pour tous les enregistrements
            table.Rows.Add(row);
        }
    }
    /// <summary>
    /// Change l'ordre de la liste de jeu par ordre ascendant ou descendant de date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListOrder.SelectedIndex == 0)
        {
            orderByAsc = true;
            //met à jour la liste des commandes de jeu de la plus vieille à la plus récente.
            ListerClients();
        }
        else
        {
            orderByAsc = false;
            //met à jour la liste des commandes de jeu de la plus récente à la plus vieille.
            ListerClients();
        }
    }
}