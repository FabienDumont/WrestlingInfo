using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WrestlingInfo.API.Entities;

public class User {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[MaxLength(200)] public string UserName { get; set; }
	[MaxLength(200)] public string Password { get; set; }

	public User(string userName, string password) {
		UserName = userName;
		Password = password;
	}
}