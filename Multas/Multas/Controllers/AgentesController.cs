using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes
        public ActionResult Index()
        {
            return View(db.Agentes.ToList());
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// mostra os dados do agente
        /// </summary>
        /// <param name="id">identifica o agente</param>
        /// <returns>devolve a view com os dados </returns>
        public ActionResult Details(int? id)
        {


            if (id == null)
            {
                // vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //este erro ocorre porque  o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }



            Agentes agentes = db.Agentes.Find(id);
            //o agente foi encontrado?
            if (agentes == null)
            {
                //o agente nao foi encontrado porque o utilizador esta a pesca
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }


        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

            /// <summary>
            /// criaçao de um novo agente
            /// </summary>
            /// <param name="agentes"> recolhe os dados do nome e da esquadra do agentee</param>
            /// <param name="fotografia">representa a fotografia que a pessoas identifica o agente</param>
            /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agentes, HttpPostedFileBase fotografia)
          
        {
            // precisamos de processar a foto
            // 1º sera que foi fornecido um ficheiro
            // 2º sera do tipo correto
            // 3º se for fo tipo correto,guarda-se
            // senao atribuise um avatar generico ao utilizador
            string caminho="";
            bool haFicheiro = false;
            //Há ficheiro
            if(fotografia == null)
            {
                //nao ha ficheiro
                // atribui se lhe o avatar
                agentes.Fotografia = "nopic.png";
            }
            else
            {
                if(fotografia.ContentType=="image/jpeg" || fotografia.ContentType == "image/png")
                {
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();

                    Guid g;
                    g = Guid.NewGuid();
                    string nome = g.ToString() + extensao;

                    caminho = Path.Combine(Server.MapPath("~/imagens"), nome);

                    agentes.Fotografia = nome;
                    haFicheiro = true;
                }
            }
            if (ModelState.IsValid)// valida se os dados fornecidos estao de acordo com as regras definifas no modelo
            {
                try
                {
                    //adiciona  o novo agente ao modelo
                    db.Agentes.Add(agentes);
                    // consolida os dados na BD
                    db.SaveChanges();
                    if (haFicheiro) fotografia.SaveAs(caminho);
                    //redireciona o utilizado para a pagina de index
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("","ocorreu um erro na escrita de dados no novo agente");
                }
            }

            return View(agentes);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                // vamos alterar esta resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //este erro ocorre porque  o utilizador anda a fazer asneiras
                return RedirectToAction("Index");
            }



            Agentes agentes = db.Agentes.Find(id);
            //o agente foi encontrado?
            if (agentes == null)
            {
                //o agente nao foi encontrado porque o utilizador esta a pesca
                //return HttpNotFound();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            //o Agente foi encontrado
            // vou salvaguardar os dados para posterior validação
            // guardar o ID do agente num cookie cifrado
            //guardar o ID numa variavel de sessão(se se usar o ASP.NET CORE esta ferramente nao existe
            // outras alternativas validas...

            Session["Agente"] = agentes.ID;
            //mostra na View os dados do Agente
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {       
            if(id == null)
            {
                // dar a volta aos espertinhos
                return RedirectToAction("Index");
            }
            // o ID nao é null
            //será o id o que eu espero?
            //vamos validar se o ID está correto
            if(id != (int)Session["Agente"])
            {
                // há aqui outro xixo esperto
                return Redirect("Index");
            }

            //procura oa gente a remover
            Agentes agentes = db.Agentes.Find(id);

            if (agentes == null)
            {
                return RedirectToAction("Index");
            }
            try   
            {

                //da ordem de remoçao do agente
                db.Agentes.Remove(agentes);
                //consolida a remoção
                db.SaveChanges();
            }
            catch (Exception)
            {
                //devem aqui ser escritas todas as instruções que são consideradas necessarias
                //imformar que houve um erro
                ModelState.AddModelError("", "Não é possivel remover o Agente"+ agentes.Nome+ ". " +
                    "Provavelmente ele tem multas associadas a ele.....");
                //redirecionar para a pagina onde o erro foi gerado
                return View(agentes);
            }

            return RedirectToAction("Index");
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
