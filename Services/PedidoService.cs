using System;
using System.Collections.Generic;
using System.Linq;
using TVPGestion_IPO.Models;
using TVPGestion_IPO.Views;

namespace TVPGestion_IPO.Services
{
    public class PedidoService
    {
        private readonly DataPersistenceService<PedidoViewModel> persistenceService;

        public PedidoService()
        {
            persistenceService = new DataPersistenceService<PedidoViewModel>("pedidos.txt");
        }

        public List<PedidoViewModel> CargarPedidos()
        {
            return persistenceService.LoadData();
        }

        public void GuardarPedidos(List<PedidoViewModel> pedidos)
        {
            persistenceService.SaveData(pedidos);
        }

        public void AgregarPedido(PedidoViewModel pedido)
        {
            persistenceService.AddItem(pedido);
        }

        public void ActualizarPedido(string id, PedidoViewModel pedidoActualizado)
        {
            persistenceService.UpdateItem(
                p => p.Id == id,
                pedidoActualizado
            );
        }

        public void EliminarPedido(string id)
        {
            persistenceService.DeleteItem(p => p.Id == id);
        }

        public string GenerarNuevoId()
        {
            var pedidos = CargarPedidos();
            if (pedidos.Count == 0)
            {
                return "PED001";
            }

            var maxId = pedidos
                .Select(p => p.Id)
                .Where(id => id.StartsWith("PED"))
                .Select(id => int.TryParse(id.Substring(3), out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"PED{(maxId + 1):D3}";
        }
    }
}