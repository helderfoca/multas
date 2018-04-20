using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using multas.Models;

namespace multas.Controllers
{
    public class AgentesController : Controller
    {
        //cria um objeto privado, que representa a base de dados
        private MultasDb db = new MultasDb();

        // GET: Agentes
        public ActionResult Index() {
            // (LINQ)db.Agentes.ToList() --> em SQL: Select * from Agentes Order by Nome
            // constroi uma lista com os dados de todos os Agentes
            // e envia-a para a View

            var listaAgentes = db.Agentes.ToList().OrderBy(a => a.Nome);

            return View(listaAgentes);
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// Apresenta os detalhes de um Agente
        /// </summary>
        /// <param name="id"> representa a PK que identifica o Agente</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            // int? - significa que pode haver valores nulos

            // protege a execução do método contra a não existência de dados
            if (id == null) {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // ou não foi introduzido um ID válido
                // ou foi introduzido um valor complemtamente errado
                return RedirectToAction("Index");
            }

            // vai procurar o Agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);

            //se o Agente não for encontrado...
            if (agente == null){
            // return HttpNotFound();
            return RedirectToAction("Index");
            }
            // envia para a View os dados do Agente
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase fileUploadFotografia)
        {
            int novoID = 0;
            //***************************************
            // proteger a geração de um novo ID
            //***************************************
            // determinar o numero de Agentes na tabela
            if (db.Agentes.Count() == 0)
            {
                novoID = 1;
            } else
            {
                novoID = db.Agentes.Max(a => a.ID) + 1;
            }
            // atribuir o ID ao novo Agente
            agente.ID = novoID;
            //******************************************
            // outra hipótese possível seria utilizar o 
            // try{ }
            // catch (Exception) { }
            //******************************************

            // var auxiliar
            string nomeFotografia = "Agente_" + novoID + ".jpg";
            string caminhoParaFotografia = Path.Combine(Server.MapPath("~/imagens/"),nomeFotografia); // incica onde a imagem vai ser guardada
           
            // verificar se chega efetivamente um ficheiro ao servidor
            if(fileUploadFotografia != null)
            {
                // guardar o nome da imagem na base de dados
                agente.Fotografia = nomeFotografia;

            } else
            {
                // não há imagem...
                ModelState.AddModelError("", "Não foi fornecida uma imagem..."); // gera MSG de erro
                return View(agente); // reenvia os dados do 'Agente' para a view
            }


            //  -verificar se o ficheiro é realmente uma imagem ---> casa
            //  -redimensionar a imagem ---> casa
            

            // ModelState.IsValid --> confronta os dados fornecidos com o modelo
            // se não respeitar as regras do modelo, rejeita os dados
            if (ModelState.IsValid) {
                try {
                    // adiciona na estrutura de dados, na memória do servidor,
                    // o objeto Agentes
                    db.Agentes.Add(agente);
                    // faz 'commit' na BD
                    db.SaveChanges();

                    // guardar a imagem no disco rígido
                    fileUploadFotografia.SaveAs(caminhoParaFotografia);

                    // redireciona o utilizador para a página de ínicio
                    return RedirectToAction("Index");
                }
                catch (Exception) {
                    // gerar uma mensagem de erro para o utilizador
                    ModelState.AddModelError("", "Ocurreu um erro não determinado na criação do novo Agente...");
                }
            }

            // se se chegar aqui, é pq aconteceu algum problema
            // devolve os dados do agente à View
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agente = db.Agentes.Find(id);
            if (agente == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                // atualiza os dados do Agente, na estrutura de dados em memória
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        /// <summary>
        /// procura os dados de um Agente,
        /// e mostra-os no ecrã
        /// </summary>
        /// <param name="id">identificador do Agente a remover</param>
        /// <returns></returns>
        public ActionResult Delete(int? id) {
            if (id == null) {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agente = db.Agentes.Find(id);
            if (agente == null) {
                // return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // POST: Agentes/Delete/5
        /// <summary>
        /// concretiza, torna definitiva (quando possível)
        /// a remoção de um Agente
        /// </summary>
        /// <param name="id">identificador do Agente a remover</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // procurar o Agente
            Agentes agente = db.Agentes.Find(id);
            try {
                // remover da memória
                db.Agentes.Remove(agente);
                // commit na BD
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception) {
                // gerar uma mensagem de erro, a ser apresentada ao utilizador
                ModelState.AddModelError("", 
                    string.Format("Não foi possível remover o Agente '{0}', porque existem {1} multas associadas a ele.",agente.Nome, agente.ListaDeMultas.Count)
                    );
            }

            // reenviar os dados para a View
            return View(agente);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
