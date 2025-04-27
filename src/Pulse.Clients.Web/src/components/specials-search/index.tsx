import React, { useState } from "react";
import { Input } from "@heroui/input";
import { Button } from "@heroui/button";

const SpecialsSearch = () => {
  const [searchTerm, setSearchTerm] = useState("");

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
  };

  const handleSearchSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    // Implement search functionality here
    console.log("Searching for:", searchTerm);
  };

  return (
    <div className="specials-search">
      <form onSubmit={handleSearchSubmit}>
        <Input
          type="text"
          value={searchTerm}
          onChange={handleSearchChange}
          placeholder="Search for specials..."
          className="search-input"
        />
        <Button type="submit" className="search-button">
          Search
        </Button>
      </form>
    </div>
  );
};

export default SpecialsSearch;
